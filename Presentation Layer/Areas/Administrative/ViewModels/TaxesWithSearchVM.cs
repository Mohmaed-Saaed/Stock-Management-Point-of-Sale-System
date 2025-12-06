

using CoreLayer.Models;

namespace PresentationLayer.Areas.Administrative.ViewModels
{
    public class TaxesWithSearchVM
    {
        public List<Tax> Taxes { get; set; } = new List<Tax>();
        public string Search { get; set; } = null!;
        public int PageId { get; set; } = 1;
        public int NoPages { get; set; }
        public string SortBy { get; set; } = null!;
    }
}
