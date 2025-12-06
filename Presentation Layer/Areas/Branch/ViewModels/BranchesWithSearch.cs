namespace PresentationLayer.Areas.Branch.ViewModels
{
    public class BranchesWithSearch
    {
        public List<CoreLayer.Models.Branch> Branches { get; set; } = new List<CoreLayer.Models.Branch>();
        public string Search { get; set; } = null!;
        public int PageId { get; set; } = 1;
        public int NoPages { get; set; }
    }
}
