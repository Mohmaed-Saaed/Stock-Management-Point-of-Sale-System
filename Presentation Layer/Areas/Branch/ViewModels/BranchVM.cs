using CoreLayer.CustomValidations;
using CoreLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Branch.ViewModels
{
    public class BranchVM
    {
        public int? BranchId { get; set; }

        [Required]
        [Display(Name = "Branch name")]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Address cannot be only whitespace.")]
        public string Address { get; set; } = null!;

        [Required]
        [InternationalPhoneNumber]
        public string PhoneNumber { get; set; } = null!;
    }
}
