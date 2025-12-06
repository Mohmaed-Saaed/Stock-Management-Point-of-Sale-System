using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces.IRepositories
{
    public interface IOperationItemRepository : IRepository<OperationItem>
    {
        /// Calculates total sales for all Items
        /// and returns the top 'count' selling items as DTOs.

        ///"count" The number of top items to retrieve (e.g., 5)
        /// returns: A list of ItemSalesSummary DTOs (BranchId, BranchName, TotalSales)
        Task<List<ItemQuantitySummaryDTO>> GetTopSellingItemsAsync(int count);
        public OperationItem? LastOrDefault(Expression<Func<OperationItem, bool>>? expression = null);
    }
}
