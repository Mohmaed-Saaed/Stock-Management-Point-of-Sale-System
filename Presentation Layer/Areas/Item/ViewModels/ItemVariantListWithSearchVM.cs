namespace PresentationLayer.Areas.Item.ViewModels
{
    public class ItemVariantListWithSearchVM<T> where T : new()
    {
        public List<T> ItemVariantList { get; set; }=new List<T> {};
        public string Search { get; set; } = null!;
        public int PageId { get; set; } = 1;
        public int NoPages { get; set; }
    }
}
