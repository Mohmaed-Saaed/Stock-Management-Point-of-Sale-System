using CoreLayer;
using CoreLayer.Models;
using InfrastructureLayer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.administrative.ViewModels;
using System.Data;
using System.Threading.Tasks;
using static CoreLayer.Models.Partner;

namespace PresentationLayer.Utility
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _Context;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly UserManager<ApplicationUser> _UserManager;
        public DbInitializer(ApplicationDbContext context,
                            RoleManager<IdentityRole> roleManager,
                            UserManager<ApplicationUser> userManager)
        {
            _Context = context;
            _RoleManager = roleManager;
            _UserManager = userManager;
        }


        public void Init()
        {

            _Context.Database.EnsureCreated();

            if (_Context.Database.GetPendingMigrations().Any())
            {
                _Context.Database.Migrate();
            }

            const int anonymousCustomerId = 1;
            const string anonymousCustomerName = "Anonymous Customer";

            bool anonymousCustomerExists = _Context.Partners
                .Any(p => p.Id == anonymousCustomerId); // Check if ID 1 exists

            if (!anonymousCustomerExists)
            {
                var anonymousCustomer = new Partner
                {
                    Id = anonymousCustomerId,
                    Name = anonymousCustomerName,
                    partnerType=PartnerType.RetailCustomer
                };

                // CRITICAL STEP: Temporarily override Identity Insertion using synchronous methods
                if (_Context.Database.IsRelational())
                {
                    _Context.Database.OpenConnection();

                    try
                    {
                        // WARNING: This uses synchronous database execution (ExecuteSqlRaw)
                        // and assumes SQL Server (Partner entity table is named 'Partners').
                        _Context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Partners ON");

                        _Context.Partners.Add(anonymousCustomer);
                        _Context.SaveChanges(); // Persist the row with ID = 1
                    }
                    finally
                    {
                        _Context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Partners OFF");
                        _Context.Database.CloseConnection();
                    }
                }
                else
                {
                    // Fallback for non-relational or different setups
                    _Context.Partners.Add(anonymousCustomer);
                }
            }

            _Context.SaveChanges();

            //if (!_Context.Users.Any() || !_Context.Roles.Any() || !_Context.Partners.Any())
            //{
                //_RoleManager.CreateAsync(new(SD.SuperAdmin)).GetAwaiter().GetResult();
                //_RoleManager.CreateAsync(new(SD.StockManager)).GetAwaiter().GetResult();
                //_RoleManager.CreateAsync(new(SD.BranchManager)).GetAwaiter().GetResult();
                //_RoleManager.CreateAsync(new(SD.Cashier)).GetAwaiter().GetResult();

                var superAdmin =  _UserManager.FindByNameAsync("SuperAdmin").GetAwaiter().GetResult();

                if (superAdmin == null)
                {
                    var result = _UserManager.CreateAsync(new ApplicationUser()
                    {
                        UserName = "SuperAdmin",
                        Email = "Email@gmail.com",
                        EmailConfirmed = true,
                    }, "SuperAdmin1!").GetAwaiter().GetResult();

                    if (result.Succeeded)
                    {
                        var Admin = SD.SuperAdmin == "Super Admin" ? "SuperAdmin" : null;
                        var getsuperAdmin = _UserManager.FindByNameAsync(Admin).GetAwaiter().GetResult();

                        if (getsuperAdmin is not null)
                        {
                            _UserManager.AddToRoleAsync(getsuperAdmin, SD.SuperAdmin).GetAwaiter().GetResult();
                            var role = _RoleManager.FindByNameAsync("Super Admin").GetAwaiter().GetResult();
                            var permissionIds = _Context.Permissions.ToList().Select(p => p.Id);

                            var rolePermissions = permissionIds.Select(pid => new RolePermission
                            {
                                RoleId = role.Id,
                                PermissionId = pid
                            });

                            _Context.RolePermissions.AddRange(rolePermissions);

                        }
                    }




                //}
               
               _Context.SaveChanges();
            }
        }
    }
}
