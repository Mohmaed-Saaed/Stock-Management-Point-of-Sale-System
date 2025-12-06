using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces;
using InfrastructureLayer.Interfaces.IRepositories;
using InfrastructureLayer.Interfaces.IRepositories.ItemVarients;
using InfrastructureLayer.Interfaces.IRepositories.Operations;
using InfrastructureLayer.Repositories;


namespace InfrastructureLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;


        // Permissions
        public IPermissionRepository Permissions { get; }
        public IRolePermissionRepository RolePermissions { get; }

        // Item Varients Repos
        public IBrandRepository Brands { get; }
        public IColorRepository Colors { get; }
        public IItemTypeRepository ItemTypes { get; }
        public ISizeRepository Sizes { get; }
        public ITargetAudienceRepository TargetAudiences { get; }

        // Operations Repos
        public IOperationRepository Operations { get; }
        public ITransferRepository Transfers { get; }
        public ISalesInvoiceRepository SalesInvoices { get; }
        public IReceiveOrderRepository ReceiveOrders { get; }

        public IBranchRepository Branches { get; }
        public IBranchItemRepository BranchItems { get; }
        public IItemRepository Items { get; }
        public IOperationItemRepository OperationItems { get; }
        public IPartnerRepository Partners { get; }
        public IApplicationUserOTPRepository ApplicationUserOTPs { get; }
        public ITaxRepository Taxes { get; set; }
        public ITaxReceiveOrderRepository TaxReceiveOrders { get; }
        public IDiscountRepository Discounts { get; }
        public IDiscountSalesInvoiceRepository DiscountSalesInvoices { get; }

        //User log-ins
        public IUserLoginHistoryRepository UserLoginHistories { get; }



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            // Initialize Item Variants Repos
            Brands = new BrandRepository(_context);
            Colors = new ColorRepository(_context);
            ItemTypes = new ItemTypeRepository(_context);
            Sizes = new SizeRepository(_context);
            TargetAudiences = new TargetAudienceRepository(_context);

            // Initialize Operations Repos
            Operations = new OperationRepository(_context);
            Transfers = new TransferRepository(_context);
            SalesInvoices = new SalesInvoiceRepository(_context);
            ReceiveOrders = new ReceiveOrderRepository(_context);

            // Initialize Core Repos
            Branches = new BranchRepository(_context);
            BranchItems = new BranchItemRepository(_context);
            Items = new ItemRepository(_context);
            OperationItems = new OperationItemRepository(_context);
            Partners = new PartnerRepository(_context);
            ApplicationUserOTPs = new ApplicationUserOTPRepository(_context);
            Taxes = new TaxRepository(_context);
            TaxReceiveOrders = new TaxReceiveOrderRepository(_context);
            Discounts = new DiscountRepository(_context);
            DiscountSalesInvoices = new DiscountSalesInvoiceRepository(_context);

            // Initialize Permissions
            Permissions = new PermissionRepository(_context);
            RolePermissions = new RolePermissionRepository(_context);
            UserLoginHistories = new UserLoginHistoryRepository(_context);

        }

        public void Dispose() => _context.Dispose();
    }
}
