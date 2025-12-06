using CoreLayer.Models.Operations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Areas.Branch.ViewModels
{
    public class ReceiveOrdersWithFiltersVM
    {
        public string Search { get; set; } = null!;
        public int BranchId { get; set; }
        public IEnumerable<SelectListItem> Branches { get; set; } = new List<SelectListItem>();
        public DateOnly? DateFilter { get; set; }
        public int? TaxRateFilter { get; set; }
        public int? DiscountRateFilter { get; set; }
        public int? GrandTotalFilter { get; set; }
        public List<ReceiveOrder> ReceiveOrders { get; set; } = new List<ReceiveOrder>();
        public int PageId { get; set; } = 1;
        public double NoPages { get; set; }
        public string SortBy { get; set; } = null!;
    }
}
