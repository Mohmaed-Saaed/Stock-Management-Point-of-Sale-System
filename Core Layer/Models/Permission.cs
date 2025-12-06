using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class Permission
    {
        //[]
        public int Id { get; set; }
        public string Name { get; set; } = null!;    
        public int? ParentId { get; set; }
        public string EnglishName { get; set; } = null!;
        public Permission? Parent { get; set; }
        public ICollection<Permission> Children { get; set; } = new List<Permission>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
