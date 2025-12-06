using CoreLayer.Models.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CoreLayer.Models.Global;


namespace CoreLayer.Models
{

    public abstract class Operation
    {
        public int Id { get; set; }
        //[Required]
        public string? Code { get; set; }

        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly Time { get; set; } = new TimeOnly();
        public Status status { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int TotalQuantity { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public double TotalAmount { get; set; }

        [Range(0, 100)]
        public int? TotalTaxesRate { get; set; }
        [Range(0, double.MaxValue)]
        public double? TotalTaxesAmount { get; set; }

        [Range(0, 100)]
        public int? TotalDiscountRate { get; set; }
        [Range(0, double.MaxValue)]
        public double? TotalDiscountAmount { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public double GrandTotal { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int RoundedGrandTotal { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public ICollection<OperationItem> OperationItems { get; set; } = new List<OperationItem>();
    }
}
