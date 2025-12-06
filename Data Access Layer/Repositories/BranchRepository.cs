using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        private readonly DbSet<Branch> _dbSet;
        public BranchRepository(ApplicationDbContext context) : base(context)
        {
            _dbSet = context.Set<Branch>();
        }

        public async Task<List<BranchSalesSummaryDTO>> GetTopSellingBranchesAsync(int count)
        {
            return await _dbSet
                // 1. Project directly to the BranchSalesSummary DTO
                .Select(branch => new BranchSalesSummaryDTO
                {
                    BranchId = branch.Id,
                    BranchName = branch.Name,
                    // 2. Perform the aggregation (Sum) on the related SalesInvoices collection
                    TotalSales = branch.SalesInvoices.Sum(si => (decimal?)si.RoundedGrandTotal) ?? 0M
                })
                // 3. Order the results by the calculated total sales
                .OrderByDescending(summary => summary.TotalSales)
                // 4. Take the specified number of results (Top N)
                .Take(count)
                // 5. Execute the query
                .ToListAsync();
        }
    }
}
