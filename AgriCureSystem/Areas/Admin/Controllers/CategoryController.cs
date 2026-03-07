using AgriCureSystem.Models;
using AgriCureSystem.Repositories.IRepositories;
using AgriCureSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriCureSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        
        private ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAsync();

            return View(categories);
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                TempData["error-notification"] = "Add failed. Please correct the highlighted errors.";
                return View(category);
            }

            await _categoryRepository.CreateAsync(category);

            
            TempData["success-notification"] = "Add Category Successfully";

            await _categoryRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var category = await _categoryRepository.GetOneAsync(e=>e.Id == id);

            if(category is not null)
            {
                return View(category);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        public async Task<IActionResult> Edit(Category category)
        {
            if(!ModelState.IsValid)
            {
                TempData["error-notification"] = "Update failed. Please correct the highlighted errors.";

                return View(category);
            }

            _categoryRepository.Edit(category);
            TempData["success-notification"] = "Update Category Successfully";
            await _categoryRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var category = await _categoryRepository.GetOneAsync(e => e.Id == id);

            if (category is not null)
            {
                _categoryRepository.Delete(category);
                TempData["success-notification"] = "Delete Category Successfully";
                await _categoryRepository.CommitAsync();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
