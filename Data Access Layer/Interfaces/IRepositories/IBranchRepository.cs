using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces.IRepositories
{
    public interface IBranchRepository : IRepository<Branch>
    {
        /// Calculates total sales for all branches by each's related SalesInvoices 
        /// and returns the top 'count' selling branches as DTOs.

        ///"count" The number of top branches to retrieve (e.g., 5)
        /// returns: A list of BranchSalesSummary DTOs (BranchId, BranchName, TotalSales)
        Task<List<BranchSalesSummaryDTO>> GetTopSellingBranchesAsync(int count);
    }
}
