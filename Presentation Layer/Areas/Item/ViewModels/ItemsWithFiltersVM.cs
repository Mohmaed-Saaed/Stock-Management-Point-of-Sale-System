using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Areas.Item.ViewModels
{
    public class ItemsWithFiltersVM
    {
        public List<CoreLayer.Models.Item> Items { get; set; }=new List<CoreLayer.Models.Item>();
        public string Search { get; set; } = null!;
        public int BrandId { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; } = new List<SelectListItem>();
        public int ColorId { get; set; }
        public IEnumerable<SelectListItem> Colors { get; set; } = new List<SelectListItem>();
        public int SizeId { get; set; }
        public IEnumerable<SelectListItem> Sizes { get; set; } = new List<SelectListItem>();
        public int TargetAudienceId { get; set; }
        public IEnumerable<SelectListItem> TargetAudiences { get; set; } = new List<SelectListItem>();
        public int ItemTypeId { get; set; }
        public IEnumerable<SelectListItem> ItemTypes { get; set; } = new List<SelectListItem>();
        public int PageId { get; set; } = 1;
        public double NoPages { get; set; }
    }
}
