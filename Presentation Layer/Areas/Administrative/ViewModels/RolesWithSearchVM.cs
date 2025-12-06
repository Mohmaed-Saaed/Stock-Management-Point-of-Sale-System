using CoreLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace PresentationLayer.Areas.Administrative.ViewModels
{
    public class RolesWithSearchVM
    {
        public List<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
        public string Search { get; set; } = null!;
        public int PageId { get; set; } = 1;
        public int NoPages { get; set; }
    }
}
