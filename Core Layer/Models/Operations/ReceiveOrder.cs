using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models.Operations
{
    public class ReceiveOrder : Operation
    {
        [Required]
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

        [Required]
        public int SupplierId { get; set; } // FK to Partner
        public Partner Supplier { get; set; } = null!;
        public ICollection<TaxReceiveOrder> TaxReceiveOrders { get; set; } = new List<TaxReceiveOrder>();
    }
}
