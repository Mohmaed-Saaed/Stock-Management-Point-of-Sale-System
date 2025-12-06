using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Identity.ViewModels
{
    public class LoginVM
    {

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        [Display(Name = "User name or email")]
        public string UserNameOrEmail { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
