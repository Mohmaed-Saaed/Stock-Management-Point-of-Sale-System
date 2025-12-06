using CoreLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using static CoreLayer.Models.Partner;

namespace PresentationLayer.Areas.Administrative.ViewModels
{
    public class PartnersWithFiltersVM
    {
        public List<Partner> Partners { get; set; }=new List<Partner>();
        public string Search { get; set; } = null!;
        public int PageId { get; set; } = 1;
        public int NoPages { get; set; }
        public string partnerType { get; set; } = null!;
        public IEnumerable<SelectListItem> PartnerTypes { get; set; } = new List<SelectListItem>();
    }
}
