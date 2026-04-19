using AgriCureSystem.Models; // اتأكد من مسار الموديل
using AgriCureSystem.Repositories.IRepositories;
using AgriCureSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AgriCureSystem.Areas.Customer.Controllers
{
    // 1. ضفنا الـ Area عشان الـ Routing يشتغل صح وميخرفش معاك
    [Area("Customer")]
    public class DiseaseScanController : Controller
    {
        private readonly IDiseaseScanRepository _scanRepository;
        private readonly HttpClient _httpClient;

        public DiseaseScanController(IDiseaseScanRepository scanRepository, HttpClient httpClient)
        {
            _scanRepository = scanRepository;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var scans = await _scanRepository.GetAsync();
            return View(scans);
        }

        [HttpGet]
        public IActionResult Scan()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Scan(string plantName, IFormFile scanImg)
        {
            if (string.IsNullOrWhiteSpace(plantName) || scanImg is null || scanImg.Length == 0)
            {
                TempData["error-notification"] = "Scan failed. Please provide plant name and an image.";
                return View();
            }

            try
            {
                // إرسال الصورة للـ AI API
                using var content = new MultipartFormDataContent();
                content.Add(new StringContent(plantName), "plant_name");

                using var apiStream = scanImg.OpenReadStream();
                var fileContent = new StreamContent(apiStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(scanImg.ContentType);
                content.Add(fileContent, "file", scanImg.FileName);

                var response = await _httpClient.PostAsync("http://127.0.0.1:8000/predict", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["error-notification"] = "Failed to get a response from the AI model.";
                    return View();
                }

                // قراءة النتيجة من الـ AI
                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var apiResult = JsonSerializer.Deserialize<AiPredictionResponse>(jsonString, options);

                // 2. حماية إضافية: نتأكد إن الـ API رجع داتا فعلاً ومفيش حاجة Null
                if (apiResult == null || apiResult.details == null)
                {
                    TempData["error-notification"] = "Failed to parse the AI prediction results.";
                    return View();
                }

                // 3. تظبيط مسار الفولدر عشان يكون مقري مرة واحدة وبشكل أنضف
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "scans");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(scanImg.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                // 4. استخدام Async في حفظ الصورة عشان منوقفش السيرفر
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await scanImg.CopyToAsync(stream); 
                }

                // تجهيز الأوبجكت للحفظ في الداتا بيز
                DiseaseScan newScan = new()
                {
                    PlantName = apiResult.plant,
                    ScanImg = fileName, 
                    Prediction = apiResult.prediction,
                    Confidence = apiResult.confidence,
                    Description = apiResult.details.Description,
                    Symptoms = apiResult.details.Symptoms,
                    Treatment = apiResult.details.Treatment
                };

                // الحفظ بالـ Repository
                await _scanRepository.CreateAsync(newScan);
                await _scanRepository.CommitAsync();

                TempData["success-notification"] = "Scan Completed and Saved Successfully";

                return RedirectToAction(nameof(Details), new { id = newScan.Id });
            }
            catch (Exception ex) // 5. استقبلنا الإيرور عشان لو حصل مشكلة نعرف نقراها
            {
                TempData["error-notification"] = $"An error occurred: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var scanResult = await _scanRepository.GetOneAsync(e => e.Id == id);

            if (scanResult is not null)
            {
                return View(scanResult);
            }

            return RedirectToAction("NotFoundPage", "Home"); // أو أي أكشن للـ Error
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var scan = await _scanRepository.GetOneAsync(e => e.Id == id);

            if (scan is not null)
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "scans", scan.ScanImg);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                _scanRepository.Delete(scan);
                await _scanRepository.CommitAsync();

                TempData["success-notification"] = "Delete Scan Record Successfully";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}