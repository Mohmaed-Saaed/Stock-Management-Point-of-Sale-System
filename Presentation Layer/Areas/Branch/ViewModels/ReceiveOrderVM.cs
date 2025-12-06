using CoreLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static CoreLayer.Models.Global;

namespace PresentationLayer.Areas.Branch.ViewModels
{

    public class ReceiveOrderVM 
    {

        public int? ReveiveOrderId{ get; set; }
        [Required]
        public int BranchId { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public DateOnly Date { get; set; }

        public string Code { get; set; } = string.Empty;
        public int? TaxId { get; set; }
        public int? DiscountPercentage { get; set; }
        public int TotalQuantity { get; set; }
        public double TotalAmount { get; set; }
        public double? TotalTaxesAmount { get; set; }
        public int? TotalTaxesRate { get; set; }
        public int? TotalDiscountRate { get; set; }
        public double? TotalDiscountAmount { get; set; }
        public double GrandTotal { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public Status Status { get; set; } 

        public ICollection<OperationItem> OperationItems { get; set; } = new List<OperationItem>();
        public List<SelectListItem> TaxList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> DiscountList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> SupplierList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> BranchList { get; set; } = new List<SelectListItem>();
    }
}
