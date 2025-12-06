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
    public class Discount
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;
        [Required]
        [Range(1, 100)]
        public int Rate { get; set; }
        public bool IsActive { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Expiration Date")]
        [FutureDate(ErrorMessage = "Expiration date must be today or a future date")]
        public DateOnly? ExpirationDate { get; set; }
        [Required]
        public int CurrentUses { get; set; } = 0;
        public int? MaximumUses { get; set; } = 0;
        public ICollection<DiscountSalesInvoice> DiscountSalesInvoices { get; set; } = new List<DiscountSalesInvoice>();
    }
}
