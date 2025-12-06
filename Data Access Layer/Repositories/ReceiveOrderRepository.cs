using CoreLayer.Models;
using CoreLayer.Models.Operations;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using InfrastructureLayer.Interfaces.IRepositories.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class ReceiveOrderRepository : Repository<ReceiveOrder>, IReceiveOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public ReceiveOrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public string GenerateCode(int BranchId)
        {
            return $"2_{BranchId}_{_context.ReceiveOrders.Count() + 1}";
        }


    }
}
