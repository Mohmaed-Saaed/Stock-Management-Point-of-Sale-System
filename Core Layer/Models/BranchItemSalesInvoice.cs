using CoreLayer.Models.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class BranchItemSalesInvoice
    {
        [Required]
        public int BranchId { get; set; }
        [Required]
        public int ItemId { get; set; }
        public BranchItem BranchItem { get; set; } = null!;
        [Required]
        public int OperationId { get; set; }
        public SalesInvoice SalesInvoice { get; set; } = null!;
    }
}
