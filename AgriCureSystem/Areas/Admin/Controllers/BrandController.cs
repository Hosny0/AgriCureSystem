using AgriCureSystem.Models;
using AgriCureSystem.Repositories.IRepositories;
using AgriCureSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AgriCureSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepository;


        public BrandController(IBrandRepository brandRepository) 
        {
            _brandRepository = brandRepository;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _brandRepository.GetAsync();

            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Brand());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                    TempData["error-notification"] = "Add failed. Please correct the highlighted errors.";
                return View(brand);
            }

            await _brandRepository.CreateAsync(brand);
            TempData["success-notification"] = "Add Brand Successfully";
            await _brandRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var brand = await _brandRepository.GetOneAsync(e => e.Id == id);

            if(brand is not null)
            {
                return View(brand);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                TempData["error-notification"] = "Update failed. Please correct the highlighted errors.";

                return View(brand);
            }

            _brandRepository.Edit(brand);
            TempData["success-notification"] = "Update Brand Successfully";

            await _brandRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var brand = await _brandRepository.GetOneAsync(e => e.Id == id);

            if (brand is not null)
            {
                _brandRepository.Delete(brand);
                TempData["success-notification"] = "Delete Brand Successfully";

                await _brandRepository.CommitAsync();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
