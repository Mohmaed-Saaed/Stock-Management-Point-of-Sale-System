using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using CoreLayer.Models.Operations;
using InfrastructureLayer.Interfaces.IRepositories;
using InfrastructureLayer.Interfaces.IRepositories.ItemVarients;
using InfrastructureLayer.Interfaces.IRepositories.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Item Varients Repos
        IBrandRepository Brands { get; }
        IColorRepository Colors { get; }
        IItemTypeRepository ItemTypes { get; }
        ISizeRepository Sizes { get; }
        ITargetAudienceRepository TargetAudiences { get; }

        // Operations Repos
        IOperationRepository Operations { get; }
        ITransferRepository Transfers { get; }
        ISalesInvoiceRepository SalesInvoices { get; }
        IReceiveOrderRepository ReceiveOrders { get; }

        IBranchRepository Branches { get; }
        IBranchItemRepository BranchItems { get; }
        IItemRepository Items { get; }
        IOperationItemRepository OperationItems { get; }
        IPartnerRepository Partners { get; }
        IApplicationUserOTPRepository ApplicationUserOTPs { get; }
        ITaxRepository Taxes { get; }
        ITaxReceiveOrderRepository TaxReceiveOrders { get; }
        IDiscountRepository Discounts { get; }
        IPermissionRepository Permissions{ get; }
        IRolePermissionRepository RolePermissions { get; }
        IUserLoginHistoryRepository UserLoginHistories { get; }
        IDiscountSalesInvoiceRepository DiscountSalesInvoices { get; }
    }
}
