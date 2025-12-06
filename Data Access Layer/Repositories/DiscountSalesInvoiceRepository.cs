using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using InfrastructureLayer.Interfaces.IRepositories.ItemVarients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class DiscountSalesInvoiceRepository : Repository<DiscountSalesInvoice>, IDiscountSalesInvoiceRepository
    {
        private readonly ApplicationDbContext _context;
        public DiscountSalesInvoiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
