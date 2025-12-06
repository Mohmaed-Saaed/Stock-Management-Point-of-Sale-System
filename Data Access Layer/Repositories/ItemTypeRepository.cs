using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using InfrastructureLayer.Interfaces.IRepositories.ItemVarients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class ItemTypeRepository : Repository<ItemType>, IItemTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public ItemTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ItemType>> GetLeafNodesAsync(Expression<Func<ItemType, bool>>? expression = null, Expression<Func<ItemType, object>>[]? include = null, bool tracked = true)
        {
            var itemTypes = await GetAsync(expression, include, tracked);
            List<ItemType> leafItemTypes = new List<ItemType>();
            int count;
            foreach (var leaf in itemTypes)
            {
                count = 0;
                foreach (var node in itemTypes)
                {
                    if (leaf.Id != node.ItemTypeId)
                        count++;
                }
                if (count == itemTypes.Count)
                    leafItemTypes.Add(leaf);
            }
            return leafItemTypes;
        }
    }
}
