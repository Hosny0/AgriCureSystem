using AgriCureSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriCureSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
   [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
