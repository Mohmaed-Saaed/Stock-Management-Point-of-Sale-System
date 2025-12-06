using CoreLayer.Models;

namespace PresentationLayer.Areas.Administrative.ViewModels
{
    public class UsersWithSearchVM
    {
        public List<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
        public string Search { get; set; } = null!;
        public int PageId { get; set; } = 1;
        public int NoPages { get; set; }
    }
}
