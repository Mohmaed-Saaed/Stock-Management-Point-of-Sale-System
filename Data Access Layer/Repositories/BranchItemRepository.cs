using CoreLayer.Models;
using CoreLayer.Models.Operations;
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
    public class BranchItemRepository : Repository<BranchItem>, IBranchItemRepository
    {
        private readonly ApplicationDbContext _context;
        public BranchItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BranchItem>> GetLowStockBranchItems()
        {
            return await GetAsync(b => b.Quantity < b.RestockThreshold, include: [b => b.Item, b => b.Branch]);
        }

        public async Task<List<SlowMovingItemDTO>> GetSlowMovingBranchItems()
        {
            DateOnly currentDateOnly = DateOnly.FromDateTime(DateTime.Now);

            // 1. Fetch all candidate BranchItems and project the data.
            var candidateItemsWithSalesData = await _context.Set<BranchItem>()
                .Where(b => b.OutDatedInMonths > 0)
                .Include(b => b.Item)
                .Include(b => b.Branch)
                .Select(bItem => new
                {
                    // Core Entity Data
                    bItem.ItemId,
                    bItem.BranchId,
                    bItem.Quantity,
                    bItem.RestockThreshold,
                    bItem.OutDatedInMonths,
                    ItemName = bItem.Item.Name,
                    BranchName = bItem.Branch.Name,

                    // Aggregate the Last Sold Date through the BranchItemSalesInvoice relation.
                    LastSoldDate = bItem.BranchItemSalesInvoices
                        .Select(link => link.SalesInvoice.Date)
                        .OrderByDescending(date => date)
                        .FirstOrDefault(), // Returns default(DateOnly) if no sales exist

                    // Aggregate the Earliest Received Date (First Entry into stock at this branch)
                    EarliestReceiveDate = _context.Set<ReceiveOrder>()
                        // Filter only ReceiveOrders for this specific branch
                        .Where(ro => ro.BranchId == bItem.BranchId)
                        // Join to the OperationItems table to find when this specific item was received
                        .SelectMany(ro => ro.OperationItems)
                        .Where(oi => oi.ItemId == bItem.ItemId)
                        .Select(oi => oi.Operation.Date) // Operation is the base class, Date is the property
                        .OrderBy(date => date)
                        .FirstOrDefault() // Returns default(DateOnly) if the item has never been received
                                          // ---------------------------------------------------------------------------------
                })
                .ToListAsync();

            // 2. Process data in C# memory to classify and map to DTOs.
            List<SlowMovingItemDTO> slowMovingItems = new List<SlowMovingItemDTO>();

            foreach (var bItemData in candidateItemsWithSalesData)
            {
                DateOnly dateToCheck;

                bool hasSold = bItemData.LastSoldDate != default(DateOnly);
                bool hasReceived = bItemData.EarliestReceiveDate != default(DateOnly);

                if (hasSold)
                {
                    // If the item has been sold, the benchmark is the LAST sale date.
                    dateToCheck = bItemData.LastSoldDate;
                }
                else if (hasReceived)
                {
                    // If the item has NEVER been sold but has been received, 
                    // the benchmark is the EARLIEST receipt date.
                    dateToCheck = bItemData.EarliestReceiveDate;
                }
                else
                {
                    // Fallback: If the item has never been sold or received, 
                    // it means it's a new entry or an error, so use current date (not slow-moving).
                    dateToCheck = currentDateOnly;
                }

                // --- CALCULATE TIME DIFFERENCES (Calculations remain the same) ---

                // Calculate whole months elapsed
                int monthsSinceLastSale = (currentDateOnly.Year * 12 + currentDateOnly.Month) -
                                          (dateToCheck.Year * 12 + dateToCheck.Month);

                // Calculate whole years elapsed
                int yearsSinceLastSale = currentDateOnly.Year - dateToCheck.Year;
                if (currentDateOnly.Month < dateToCheck.Month ||
                    (currentDateOnly.Month == dateToCheck.Month && currentDateOnly.Day < dateToCheck.Day))
                {
                    yearsSinceLastSale--;
                }
                yearsSinceLastSale = Math.Max(0, yearsSinceLastSale);

                // Calculate days elapsed (the most precise metric)
                int daysSinceLastSale = (int)(currentDateOnly.ToDateTime(TimeOnly.MinValue) - dateToCheck.ToDateTime(TimeOnly.MinValue)).TotalDays;

                // 3. Check the slow-moving condition: Is the time elapsed older than the allowed threshold?
                if (monthsSinceLastSale > bItemData.OutDatedInMonths)
                {
                    // Map the data to the DTO
                    slowMovingItems.Add(new SlowMovingItemDTO
                    {
                        BranchItemId = bItemData.ItemId,
                        ItemName = bItemData.ItemName,
                        BranchName = bItemData.BranchName,
                        Quantity = bItemData.Quantity,
                        OutDatedInMonths = bItemData.OutDatedInMonths,

                        // Assign all three calculated metrics
                        MonthsSinceLastSale = monthsSinceLastSale,
                        DaysSinceLastSale = daysSinceLastSale,
                        YearsSinceLastSale = yearsSinceLastSale
                    });
                }
            }

            return slowMovingItems;
        }
    }
}
