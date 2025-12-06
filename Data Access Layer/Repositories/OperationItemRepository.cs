using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class OperationItemRepository : Repository<OperationItem>, IOperationItemRepository
    {
        private readonly ApplicationDbContext _context;
        public OperationItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public OperationItem? LastOrDefault(Expression<Func<OperationItem, bool>>? expression = null)
        {
          return expression != null ? _context.OperationItems.LastOrDefault(expression) : _context.OperationItems.LastOrDefault();
        }
        public async Task<List<ItemQuantitySummaryDTO>> GetTopSellingItemsAsync(int count)
        {
            var operationItems = await _context.OperationItems.Include(o => o.Item).ToListAsync();
            List<ItemQuantitySummaryDTO> items = new List<ItemQuantitySummaryDTO>();

            foreach (var opItem in operationItems)
            {
                if (items.Find(i => i.ItemId == opItem.ItemId) is ItemQuantitySummaryDTO i)
                {
                    i.TotalQuantity += (decimal)opItem.TotalPrice;
                }
                else
                {
                    items.Add(new ItemQuantitySummaryDTO
                    {
                        ItemId = opItem.ItemId,
                        ItemName = opItem.Item.Name,
                        TotalQuantity = (decimal)opItem.TotalPrice
                    }
                        );
                }
            }
            items = items.OrderByDescending(i => i.TotalQuantity).Take(count).ToList();
            return items;
        }
    }
}
