using CoreLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Areas.Administrative.ViewModels
{
    public class DiscountsWithFiltersVM
    {
        public List<Discount> Discounts { get; set; } = new List<Discount>();
        public int PageId { get; set; } = 1;
        public int NoPages { get; set; }
        public string Search { get; set; } = null!;
        public int? MinRate { get; set; }
        public bool? IsActiveFilter { get; set; }
        public DateOnly? ExpDateFilter { get; set; }
        public int? MinNoUses { get; set; }
        public string SortBy { get; set; } = null!;
    }
}
