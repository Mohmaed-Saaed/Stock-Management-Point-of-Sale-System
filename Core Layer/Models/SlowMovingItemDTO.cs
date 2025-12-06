using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    /// DTO to carry BranchItem data along with calculated slow-moving metrics.
    public class SlowMovingItemDTO
    {
        public int BranchItemId { get; set; }

        // Data from the BranchItem entity
        public string ItemName { get; set; }
        public string BranchName { get; set; }
        public int Quantity { get; set; }

        // Calculated metrics
        public int OutDatedInMonths { get; set; }
        public int YearsSinceLastSale { get; set; }
        public int MonthsSinceLastSale { get; set; }
        public int DaysSinceLastSale { get; set; } 
    }
}
