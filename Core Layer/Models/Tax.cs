using CoreLayer.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class Tax
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;
        [Required]
        [Range(0, 100, ErrorMessage = "Discount is written in percentage values from 0 to 100")]
        public int Rate { get; set; }
        public ICollection<TaxReceiveOrder> TaxReceiveOrders { get; set; } = new List<TaxReceiveOrder>();
    }
}
