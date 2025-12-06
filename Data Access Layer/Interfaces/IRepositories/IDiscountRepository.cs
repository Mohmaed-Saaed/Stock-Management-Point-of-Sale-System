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
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<List<Discount>> GetActiveDiscountsAsync(Expression<Func<Discount, bool>>? expression = null, Expression<Func<Discount, object>>[]? include = null, bool tracked = true);
        Task<bool> IncrementDiscountsUses(int id);
    }
}
