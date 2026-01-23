using AgriCureSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgriCureSystem.ViewModels
{
    public class CategoryWithBrandVM
    {
        public List<SelectListItem> Categories { get; set; } = null!;
        public List<SelectListItem> Brands { get; set; } = null!;
        public Product? Product { get; set; }
    }
}
