using CoreLayer.Models.Operations;

namespace PresentationLayer.Areas.Sales.DTOs
{
    public class CreateSalesInvoiceDTO
    {
        public int BranchId { get; set; }
        public int RetailCustomerId { get; set; }
        public string ApplicationUserId { get; set; } = null!;

        public int TotalQuantity { get; set; }
        public double TotalAmount { get; set; }
        public int TotalDiscountRate { get; set; }
        public double TotalDiscountAmount { get; set; }
        public double GrandTotal { get; set; }
        public int RoundedGrandTotal { get; set; }
        public int PaidCash { get; set; }
        public int Change { get; set; }

        public List<int> GeneralDiscounts { get; set; } = new();

        public List<CreateOperationItemDTO> OperationItems { get; set; } = new();
    }

    public class CreateOperationItemDTO
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double SellingPrice { get; set; }
        public double DiscountPrice { get; set; }
        public int DiscountRate { get; set; }
    }

}
