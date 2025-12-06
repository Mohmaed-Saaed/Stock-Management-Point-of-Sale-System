using CoreLayer.CustomValidations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static CoreLayer.Models.Partner;

namespace PresentationLayer.Areas.administrative.ViewModels
{
    public class PartnerVM
    {

        public int? Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;
        [EmailAddress]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Email cannot be only whitespace.")]
        public string? Email { get; set; } = string.Empty;
        [Required]
        public PartnerType partnerType { get; set; }
        [InternationalPhoneNumber]
        public string? PhoneNumber { get; set; }

        public List<SelectListItem> PartnerList = new List<SelectListItem>() {
        {new SelectListItem{Text = "Supplier" , Value = "1"}},
             {new SelectListItem{Text = "Retail Customer"  , Value = "2"}}
        };

    }
}
