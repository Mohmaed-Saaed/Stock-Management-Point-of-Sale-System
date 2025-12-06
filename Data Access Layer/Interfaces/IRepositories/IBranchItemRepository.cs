using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces.IRepositories
{
    public interface IBranchItemRepository : IRepository<BranchItem>
    {
        Task<List<BranchItem>> GetLowStockBranchItems();
        Task<List<SlowMovingItemDTO>> GetSlowMovingBranchItems();
    }
}
