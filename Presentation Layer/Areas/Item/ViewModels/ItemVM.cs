using CoreLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Areas.Item.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Stock.ViewModels
{
    public class ItemVM
    {
        public int Id { get; set; } 
        [Required]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Barcode cannot be only whitespace.")]
        public string Barcode { get; set; } = null!;
        public IFormFile? formFile { get; set; }

        [RegularExpression(@"^(?!\s)(?!\s+$).+", ErrorMessage = "File name cannot start with spaces or be only whitespace.")]
        public string? Image { get; set; } = null!;
        public bool deleteImage { get; set; }
        [Required]
        public int SizeId { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        public int ColorId { get; set; }

        [Required]
        public int ItemTypeId { get; set; }

        [Required]
        public int TargetAudienceId { get; set; }

        public IEnumerable<SelectListItem> BrandsList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ColorsList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ItemTypesList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> SizesList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TargetAudiencesList { get; set; } = new List<SelectListItem>();
    }
}
