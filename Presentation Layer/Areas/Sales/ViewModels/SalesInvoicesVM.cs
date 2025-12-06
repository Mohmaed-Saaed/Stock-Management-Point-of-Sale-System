using CoreLayer.Models;
using CoreLayer.Models.Operations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Areas.Sales.ViewModels
{
    public class SalesInvoicesVM
    {
        public string Search { get; set; } = null!;
        public int BranchId { get; set; }
        public IEnumerable<SelectListItem> Branches { get; set; } = new List<SelectListItem>();
        public DateOnly? DateFilter { get; set; }
        public int? TotalQtyFilter { get; set; }
        public int? GrandTotalFilter { get; set; }
        public List<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
        public int PageId { get; set; } = 1;
        public double NoPages { get; set; }
        public string SortBy { get; set; } = null!;
    }
}
