using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    // Not a Db Table
    // DTO used to transfer aggregated sales data (Branch ID and Total Sales) 
    // from the branch's repository layer to the controller and the view.
    public class BranchSalesSummaryDTO
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = null!;
        public decimal TotalSales { get; set; }
    }
}
