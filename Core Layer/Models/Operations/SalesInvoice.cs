using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models.Operations
{
    public class SalesInvoice : Operation
    {
        [Required]
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

        [Required]
        public int RetailCustomerId { get; set; }  // FK to Partner
        public Partner RetailCustomer { get; set; } = null!;
        public int PaidCash { get; set; }
        public int Change { get; set; }
        public ICollection<DiscountSalesInvoice> DiscountSalesInvoices { get; set; } = new List<DiscountSalesInvoice>();
        public ICollection<BranchItemSalesInvoice> BranchItemSalesInvoices { get; set; } = new List<BranchItemSalesInvoice>();
    }
}
