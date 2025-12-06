using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Identity.ViewModels
{
    public class ResetPasswordVM
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        [Required]
        public int OTPNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
