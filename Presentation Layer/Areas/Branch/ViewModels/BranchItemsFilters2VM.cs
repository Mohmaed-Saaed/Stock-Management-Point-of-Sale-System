using CoreLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Areas.Branch.ViewModels
{
    public class BranchItemsFilters2VM
    {
        public int Id { get; set; }
        public string Search { get; set; } = null!;
        public IEnumerable<SelectListItem> Branches { get; set; } = new List<SelectListItem>();
        public int? QuantityFilter { get; set; }
        public int? RestockThresholdFilter { get; set; }
        public int? BuyingPriceAvgFilter { get; set; }
        public int? LastBuyingPriceFilter { get; set; }
        public int? SellingPriceFilter { get; set; }
        public int? DiscountRateFilter { get; set; }
        public int? OutDatedInMonthsFilter { get; set; }
        public int PageId { get; set; } = 1;
        public double NoPages { get; set; }
        public string SortBy { get; set; } = null!;

        public List<BranchItem> BranchItems { get; set; } = new List<BranchItem>();
    }
}
