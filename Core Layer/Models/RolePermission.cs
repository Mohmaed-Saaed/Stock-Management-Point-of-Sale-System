using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class RolePermission
    {
        //[Required]
        public string RoleId { get; set; } = null!;
        public IdentityRole Role { get; set; } = null!;
        //[Required]
        public int PermissionId { get; set; } 
        public Permission Permission { get; set; } = null!;
    }

}