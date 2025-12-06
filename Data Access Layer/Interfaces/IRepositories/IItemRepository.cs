using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces.IRepositories
{
    public interface IItemRepository : IRepository<Item>
    {
        bool IsNameExist(string name, int? id);
        bool IsBarcodeExist(string name, int? id);
    }
}
