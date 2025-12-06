using CoreLayer.Models.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class  Branch
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Address cannot be only whitespace.")]
        public string Address { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        // Many-to-Many: Branch has many Items (Bridge Table Needed)
        public ICollection<BranchItem> BranchItems { get; set; } = new List<BranchItem>();

        // One-to-Many: Branch has many cashiers
        public ICollection<ApplicationUser> Cashiers { get; set; } = new List<ApplicationUser>();

        // One-to-Many: Branch has many Operations
        public ICollection<SalesInvoice>? SalesInvoices { get; set; } = new List<SalesInvoice>();
        public ICollection<ReceiveOrder>? ReceiveOrders { get; set; } = new List<ReceiveOrder>();
        public ICollection<Transfer>? OutgoingTransfers { get; set; } = new List<Transfer>();  // Transfers FROM this branch
        public ICollection<Transfer>? IncomingTransfers { get; set; } = new List<Transfer>();  // Transfers TO this branch
    }
}
