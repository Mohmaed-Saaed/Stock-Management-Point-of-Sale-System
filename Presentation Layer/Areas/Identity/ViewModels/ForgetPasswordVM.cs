using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Identity.ViewModels
{
    public class ForgetPasswordVM
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        public string UserNameOrEmail { get; set; } = null!;
    }
}
