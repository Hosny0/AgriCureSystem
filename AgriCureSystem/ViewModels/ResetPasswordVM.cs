using System.ComponentModel.DataAnnotations;

namespace AgriCureSystem.ViewModels
{
    public class ResetPasswordVM
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; } = string.Empty;
        public string UserId { get; set; }
    }
}
