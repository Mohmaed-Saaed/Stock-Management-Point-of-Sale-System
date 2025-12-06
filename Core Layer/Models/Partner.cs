using CoreLayer.CustomValidations;
using CoreLayer.Models.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class Partner
    {
        public enum PartnerType
        {
            Supplier = 1,
            RetailCustomer = 2
        }
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;

        [EmailAddress]
        public string? Email { get; set; } = null!;
        [Required]
        public PartnerType partnerType { get; set; }

        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; } = string.Empty;

        public ICollection<SalesInvoice>? SalesInvoices { get; set; } = new List<SalesInvoice>();
        public ICollection<ReceiveOrder>? ReceiveOrders { get; set; } = new List<ReceiveOrder>();
    }
}
