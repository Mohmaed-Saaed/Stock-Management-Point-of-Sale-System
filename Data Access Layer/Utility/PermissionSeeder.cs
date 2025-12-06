using CoreLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Utility
{
    public static class PermissionSeeder
    {
        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<Permission>().HasData(
             new Permission { Id = 1, Name = "System", EnglishName = "System" },
             
            #region Branch
             new Permission { Id = 10, Name = "Branch", EnglishName = "Branch", ParentId = 1 },
             
             // Stock
             new Permission { Id = 100, Name = "Stock", EnglishName = "Stock", ParentId = 10 },
             new Permission { Id = 101, Name = "Stock.View", EnglishName = "View Stock", ParentId = 100 },
             new Permission { Id = 102, Name = "Stock.Add", EnglishName = "Add Stock", ParentId = 100 },
             new Permission { Id = 103, Name = "Stock.Edit", EnglishName = "Edit Stock", ParentId = 100 },
             new Permission { Id = 104, Name = "Stock.Delete", EnglishName = "Delete Stock", ParentId = 100 },

             // Receive Order
             new Permission { Id = 120, Name = "ReceiveOrder", EnglishName = "Receive Order", ParentId = 10 },
             new Permission { Id = 121, Name = "ReceiveOrder.View", EnglishName = "View Receive Order", ParentId = 120 },
             new Permission { Id = 122, Name = "ReceiveOrder.Add", EnglishName = "Add Receive Order", ParentId = 120 },
             new Permission { Id = 123, Name = "ReceiveOrder.Edit", EnglishName = "Edit Receive Order", ParentId = 120 },
             new Permission { Id = 124, Name = "ReceiveOrder.Delete", EnglishName = "Delete Receive Order", ParentId = 120 },
             new Permission { Id = 125, Name = "ReceiveOrder.Confirm", EnglishName = "Confirm Receive Order", ParentId = 120 },
            #endregion

            #region Item

             // Item
             new Permission { Id = 11, Name = "Item", EnglishName = "Item", ParentId = 1 },

             new Permission { Id = 140, Name = "ClothingItem", EnglishName = "Clothing Item", ParentId = 11 },
             new Permission { Id = 141, Name = "ClothingItem.View", EnglishName = "View ClothingItem", ParentId = 140 },
             new Permission { Id = 142, Name = "ClothingItem.Add", EnglishName = "Add ClothingItem", ParentId = 140 },
             new Permission { Id = 143, Name = "ClothingItem.Edit", EnglishName = "Edit ClothingItem", ParentId = 140 },
             new Permission { Id = 144, Name = "ClothingItem.Delete", EnglishName = "Delete ClothingItem", ParentId = 140 },
             new Permission { Id = 145, Name = "ClothingItem.BranchItem", EnglishName = "Add Data To A Branch", ParentId = 140 },

             new Permission { Id = 160, Name = "Color", EnglishName = "Color", ParentId = 11 },
             new Permission { Id = 161, Name = "Color.View", EnglishName = "View Color", ParentId = 160 },
             new Permission { Id = 162, Name = "Color.Add", EnglishName = "Add Color", ParentId = 160 },
             new Permission { Id = 163, Name = "Color.Edit", EnglishName = "Edit Color", ParentId = 160 },
             new Permission { Id = 164, Name = "Color.Delete", EnglishName = "Delete Color", ParentId = 160 },

             new Permission { Id = 180, Name = "Size", EnglishName = "Size", ParentId = 11 },
             new Permission { Id = 181, Name = "Size.View", EnglishName = "View Size", ParentId = 180 },
             new Permission { Id = 182, Name = "Size.Add", EnglishName = "Add Size", ParentId = 180 },
             new Permission { Id = 183, Name = "Size.Edit", EnglishName = "Edit Size", ParentId = 180 },
             new Permission { Id = 184, Name = "Size.Delete", EnglishName = "Delete Size", ParentId = 180 },
                                   
             new Permission { Id = 200, Name = "ItemType", EnglishName = "Item Type", ParentId = 11 },
             new Permission { Id = 201, Name = "ItemType.View", EnglishName = "View Item Type", ParentId = 200 },
             new Permission { Id = 202, Name = "ItemType.Add", EnglishName = "Add Item Type", ParentId = 200 },
             new Permission { Id = 203, Name = "ItemType.Edit", EnglishName = "Edit Item Type", ParentId = 200 },
             new Permission { Id = 204, Name = "ItemType.Delete", EnglishName = "Delete Item Type", ParentId = 200 },

             new Permission { Id = 220, Name = "TargetAudience", EnglishName = "Target Audience", ParentId = 11 },
             new Permission { Id = 221, Name = "TargetAudience.View", EnglishName = "View Target Audience", ParentId = 220 },
             new Permission { Id = 222, Name = "TargetAudience.Add", EnglishName = "Add Target Audience", ParentId = 220 },
             new Permission { Id = 223, Name = "TargetAudience.Edit", EnglishName = "Edit Target Audience", ParentId = 220 },
             new Permission { Id = 224, Name = "TargetAudience.Delete", EnglishName = "Delete Target Audience", ParentId = 220 },

             new Permission { Id = 240, Name = "Brand", EnglishName = "Brand", ParentId = 11 },
             new Permission { Id = 241, Name = "Brand.View", EnglishName = "View Brand", ParentId = 240 },
             new Permission { Id = 242, Name = "Brand.Add", EnglishName = "Add Brand", ParentId = 240 },
             new Permission { Id = 243, Name = "Brand.Edit", EnglishName = "Edit Brand", ParentId = 240 },
             new Permission { Id = 244, Name = "Brand.Delete", EnglishName = "Delete Brand", ParentId = 240 },

            #endregion
            #region Administrative
             new Permission { Id = 12, Name = "Administrative", EnglishName = "Administrative", ParentId = 1 },
             // Partner
             new Permission { Id = 260, Name = "Partner", EnglishName = "Partner", ParentId = 12 },
             new Permission { Id = 261, Name = "Partner.View", EnglishName = "View Partner", ParentId = 260 },
             new Permission { Id = 262, Name = "Partner.Add", EnglishName = "Add Partner", ParentId = 260 },
             new Permission { Id = 263, Name = "Partner.Edit", EnglishName = "Edit Partner", ParentId = 260 },
             new Permission { Id = 264, Name = "Partner.Delete", EnglishName = "Delete Partner", ParentId = 260 },

             // User
             new Permission { Id = 280, Name = "User", EnglishName = "User", ParentId = 12 },
             new Permission { Id = 281, Name = "User.View", EnglishName = "View User", ParentId = 280 },
             new Permission { Id = 282, Name = "User.Add", EnglishName = "Add User", ParentId = 280 },
             new Permission { Id = 283, Name = "User.Edit", EnglishName = "Edit User", ParentId = 280 },
             new Permission { Id = 284, Name = "User.Delete", EnglishName = "Delete User", ParentId = 280 },

            // Role
             new Permission { Id = 300, Name = "Role", EnglishName = "Role", ParentId = 12 },
             new Permission { Id = 301, Name = "Role.View", EnglishName = "View Role", ParentId = 300 },
             new Permission { Id = 302, Name = "Role.Add", EnglishName = "Add Role", ParentId = 300 },
             new Permission { Id = 303, Name = "Role.Edit", EnglishName = "Edit Role", ParentId = 300 },
             new Permission { Id = 304, Name = "Role.Delete", EnglishName = "Delete Role", ParentId = 300 },

             // Setting
             //new Permission { Id = 320, Name = "Setting", EnglishName = "Setting", ParentId = 12 },

             //Tax
             new Permission { Id = 340, Name = "Tax", EnglishName = "Tax", ParentId = 12 },
             new Permission { Id = 341, Name = "Tax.View", EnglishName = "View Tax", ParentId = 340 },
             new Permission { Id = 342, Name = "Tax.Add", EnglishName = "Add Tax", ParentId = 340 },
             new Permission { Id = 343, Name = "Tax.Edit", EnglishName = "Edit Tax", ParentId = 340 },
             new Permission { Id = 344, Name = "Tax.Delete", EnglishName = "Delete Tax", ParentId = 340 },

             //Discount
             new Permission { Id = 360, Name = "Discount", EnglishName = "Discount", ParentId = 12 },
             new Permission { Id = 361, Name = "Discount.View", EnglishName = "View Discount", ParentId = 360 },
             new Permission { Id = 362, Name = "Discount.Add", EnglishName = "Add Discount", ParentId = 360 },
             new Permission { Id = 363, Name = "Discount.Edit", EnglishName = "Edit Discount", ParentId = 360 },
             new Permission { Id = 364, Name = "Discount.Delete", EnglishName = "Delete Discount", ParentId = 360 },

             //UserLogins
             new Permission { Id = 380, Name = "UserLoginHistory", EnglishName = "User Login History", ParentId = 12 },
             new Permission { Id = 381, Name = "UserLoginHistory.View", EnglishName = "View UserLoginHistory", ParentId = 380 },

             #endregion

            #region Sales
             new Permission { Id = 13, Name = "Sales", EnglishName = "Sales", ParentId = 1 },

             // POS
             new Permission { Id = 400, Name = "POS", EnglishName = "POS", ParentId = 13 },

            // SalesInvoice
              new Permission { Id = 420, Name = "SalesInvoice", EnglishName = "Sales Invoice", ParentId = 13 },
              new Permission { Id = 421, Name = "SalesInvoice.View", EnglishName = "View Sales Invoice", ParentId = 420 },
              new Permission { Id = 422, Name = "SalesInvoice.Print", EnglishName = "Print Sales Invoice", ParentId = 420 },
              #endregion

            #region Dashboard
              new Permission { Id = 14, Name = "Dashboard", EnglishName = "Dashboard", ParentId = 1 },
            // DashboardView
              new Permission { Id = 440, Name = "Dashboard.View", EnglishName = "View Dashboard", ParentId = 14 }
            #endregion

            );
        }
    }
}
