using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Branch.ViewModels
{
    public class BranchItemDTO
    {
        public int ItemId { get; set; }
        public int BranchId{ get; set; }
        public int Quantity { get; set; }
        public double BuyingPriceAvg { get; set; }
        public double LastBuyingPrice { get; set; }
        public double SellingPrice { get; set; }
        [Range(0, 100, ErrorMessage = "Discount is written in percentage values from 0 to 100")]
        public int DiscountRate { get; set; }
        public int RestockThreshold { get; set; }
        public int OutDatedInMonths { get; set; }
    }
}
