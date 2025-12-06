using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class TaxRepository : Repository<Tax>, ITaxRepository
    {
        private readonly ApplicationDbContext _context;
        public TaxRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
