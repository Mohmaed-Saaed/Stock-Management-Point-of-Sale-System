using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class OperationItem
    {
        public int Id { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        // Price per unit at the moment of sale
        [Required]
        [Range(0.0, double.MaxValue)]
        public double SellingPrice { get; set; }

        // Buying Price per unit at the moment of Receive Order Operation
        [Required]
        [Range(0.0, double.MaxValue)]
        public double BuyingPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Discount is written in percentage values from 0 to 100")]
        public int? DiscountRate { get; set; }

        // Sales Invoice: Quantity * Selling Price * (1 - DiscountRate/100)
        // Receive Order: Quantity * Last Buying Price * (1 + Tax/100)
        [Required]
        [Range(0.0, double.MaxValue)]
        public double TotalPrice { get; set; }

        [Required]
        public int OperationId { get; set; }
        public Operation Operation { get; set; } = null!;

        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
    }
}
