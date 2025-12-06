using CoreLayer.Models;
using CoreLayer.Models.Operations;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories.Operations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InfrastructureLayer.Repositories
{
    public class SalesInvoiceRepository : Repository<SalesInvoice>, ISalesInvoiceRepository
    {
        private readonly ApplicationDbContext _context;
        public SalesInvoiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Creates and persists the BranchItemSalesInvoice records for a newly created invoice.
        public async Task AddBranchItemTrackingAsync(SalesInvoice invoice)
        {
            // 1. Get the ItemIds of OperationItems.
            var soldItemIds = invoice.OperationItems.Select(oi => oi.ItemId).Distinct().ToList();

            // 2. Fetch the BranchItems by ItemIds in same Branch
            var branchItems = await _context.Set<BranchItem>()
                .Where(bi => bi.BranchId == invoice.BranchId && soldItemIds.Contains(bi.ItemId))
                .AsNoTracking()
                .ToListAsync();

            // 3. Create the list of bridge entities to be added.
            var newTrackingLinks = branchItems.Select(bi => new BranchItemSalesInvoice
            {
                BranchId = invoice.BranchId,
                ItemId = bi.ItemId,
                OperationId = invoice.Id
            }).ToList();

            // 4. Add all new links to the bridge table and save changes.
            await _context.Set<BranchItemSalesInvoice>().AddRangeAsync(newTrackingLinks);
            await _context.SaveChangesAsync();

            // 5. Explicitly detach the BranchItem entities from the change tracker.
            foreach (var branchItem in branchItems)
            {
                // Check if the entity is currently being tracked (even if AsNoTracking was used, 
                var trackedEntry = _context.Entry(branchItem);
                if (trackedEntry.State != EntityState.Detached)
                {
                    // Force the entity to be detached from the DbContext.
                    trackedEntry.State = EntityState.Detached;
                }
            }
        }

        public string GenerateCode(int BranchId)
        {
            return $"1_{BranchId}_{_context.SalesInvoices.Count() + 1}";
        }
    }
}
