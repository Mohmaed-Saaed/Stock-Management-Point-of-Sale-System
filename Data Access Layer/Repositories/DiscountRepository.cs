using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces;
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
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        private readonly ApplicationDbContext _context;
        public DiscountRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Discount>> GetActiveDiscountsAsync(Expression<Func<Discount, bool>>? expression = null, Expression<Func<Discount, object>>[]? include = null, bool tracked = true)
        {
            var discounts = await GetAsync(expression, include, tracked);
            List<Discount> activeDiscounts = new List<Discount>();
            foreach (var discount in discounts)
            {
                if (discount.IsActive)
                {
                    // Both Conditions Exist, Both Must satisfy
                    if (discount.ExpirationDate is not null && discount.MaximumUses != 0)
                    {
                        if (discount.ExpirationDate.Value >= DateOnly.FromDateTime(DateTime.Now) && discount.CurrentUses < discount.MaximumUses)
                            activeDiscounts.Add(discount);
                        else
                        {
                            discount.IsActive = false;
                            var result = await CommitAsync();
                            if (!result)
                                return null;
                            continue;
                        }
                    }

                    if (discount.MaximumUses != 0 && discount.CurrentUses < discount.MaximumUses)
                        activeDiscounts.Add(discount);

                    else if (discount.ExpirationDate is not null && discount.ExpirationDate.Value >= DateOnly.FromDateTime(DateTime.Now))
                        activeDiscounts.Add(discount);

                    else if (discount.ExpirationDate is null && discount.MaximumUses == 0)
                        activeDiscounts.Add(discount);

                    else
                    {
                        discount.IsActive = false;
                        var result = await CommitAsync();
                        if (!result)
                            return null;
                    }
                }
            }
            return activeDiscounts;
        }

        public async Task<bool> IncrementDiscountsUses(int id)
        {
            try
            {
                if ((await _context.Discounts.FindAsync(id)) is Discount discount)
                {
                    discount.CurrentUses++;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
