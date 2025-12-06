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
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        private readonly ApplicationDbContext _context;
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public bool IsNameExist(string name , int? id)
        {
            return _context.Items.Any(i => i.Name == name && i.Id != id);
        }

        public bool IsBarcodeExist(string barcode, int? id)
        {
            return _context.Items.Any(i => i.Barcode == barcode && i.Id != id);
        }
    }
}
