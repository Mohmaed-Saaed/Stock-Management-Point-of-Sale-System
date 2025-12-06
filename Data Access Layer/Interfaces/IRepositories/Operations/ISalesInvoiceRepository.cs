using CoreLayer.Models.ItemVarients;
using CoreLayer.Models.Operations;
using InfrastructureLayer.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces.IRepositories.Operations
{
    public interface ISalesInvoiceRepository : IRepository<SalesInvoice>
    {
        Task AddBranchItemTrackingAsync(SalesInvoice invoice);
        string GenerateCode(int BranchId);
    }
}
