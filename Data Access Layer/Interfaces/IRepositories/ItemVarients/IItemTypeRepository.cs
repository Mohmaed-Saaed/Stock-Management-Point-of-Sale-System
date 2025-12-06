using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces.IRepositories.ItemVarients
{
    public interface IItemTypeRepository : IRepository<ItemType>
    {
        Task<List<ItemType>> GetLeafNodesAsync(Expression<Func<ItemType, bool>>? expression = null, Expression<Func<ItemType, object>>[]? include = null, bool tracked = true);
    }
}
