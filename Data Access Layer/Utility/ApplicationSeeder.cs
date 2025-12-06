using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using CoreLayer.Models.Operations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static CoreLayer.Models.Global;

namespace InfrastructureLayer.Utility
{
    internal static class ApplicationSeeder
    {
        private static ModelBuilder? _builder;
        public static void Seed(ModelBuilder builder)
        {
            _builder = builder;
            SeedBranch();
            SeedPartner();
            SeedDiscounts();
            SeedTax();
            SeedTargetAudiences();
            SeedSizes();
            SeedItemTypes();
            SeedColors();
            SeedBrands();
            SeedUsers();
            SeedRoles();
            SeedUserRoles();
            SeedItems();
            SeedBranchItem();
            SeedReceiveOrder();
            SeedSalesInvoices();
        }
        public static void SeedBranch()
        {
            _builder!.Entity<Branch>().HasData(
                new Branch
                {
                    Id = 1,
                    Name = "Main Branch",
                    Address = "123 Main Street, City Center",
                    PhoneNumber = "+201017671158",
                    CreatedDate = new DateTime(2023, 01, 01)
                },
                new Branch
                {
                    Id = 2,
                    Name = "East Side Branch",
                    Address = "456 East Street, East Town",
                    PhoneNumber = "+442079460958",
                    CreatedDate = new DateTime(2023, 02, 01)
                },
                new Branch
                {
                    Id = 3,
                    Name = "West End Branch",
                    Address = "789 West Avenue, Westside",
                    PhoneNumber = "+12025550123",
                    CreatedDate = new DateTime(2023, 03, 01)
                }
            );
        }
        public static void SeedPartner()
        {
            _builder!.Entity<Partner>().HasData(
                new Partner
                {
                    Id = 1,
                    Name = "Anonymous Customer",
                    partnerType = Partner.PartnerType.RetailCustomer
                },
                new Partner
                {
                    Id = 2,
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    partnerType = Partner.PartnerType.RetailCustomer,
                    PhoneNumber = "+201112223344"
                },
                new Partner
                {
                    Id = 3,
                    Name = "XYZ Retail",
                    Email = "sales@xyzretail.com",
                    partnerType = Partner.PartnerType.RetailCustomer,
                    PhoneNumber = "+12025550123"
                },
                new Partner
                {
                    Id = 4,
                    Name = "ABC Suppliers",
                    Email = "contact@abc.com",
                    partnerType = Partner.PartnerType.Supplier,
                    PhoneNumber = "+201001112233"
                }
            );
        }
        public static void SeedTax()
        {
            _builder!.Entity<Tax>().HasData(
                 new Tax
                 {
                     Id = 1,
                     Name = "VAT",
                     Rate = 14
                 },
              new Tax
              {
                  Id = 2,
                  Name = "Service Tax",
                  Rate = 10
              },
              new Tax
              {
                  Id = 3,
                  Name = "Luxury Tax",
                  Rate = 5
              }
                  );
        }
        public static void SeedDiscounts()
        {
            _builder!.Entity<Discount>().HasData(
                new Discount
                {
                    Id = 1,
                    Name = "New Year Offer",
                    Rate = 15,
                    IsActive = true,
                    ExpirationDate = new DateOnly(2028, 1, 1),
                    CurrentUses = 0,
                    MaximumUses = 100
                },
                new Discount
                {
                    Id = 2,
                    Name = "Black Friday",
                    Rate = 25,
                    IsActive = true,
                    ExpirationDate = new DateOnly(2025, 11, 30),
                    CurrentUses = 0,
                    MaximumUses = 500
                },
                new Discount
                {
                    Id = 3,
                    Name = "Loyalty Discount",
                    Rate = 2,
                    IsActive = true,
                    ExpirationDate = null, // never expires
                    CurrentUses = 0,
                    MaximumUses = null // unlimited
                }
            );
        }
        public static void SeedBrands()
        {
            _builder!.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "Nike", Image = "nike.png" },
                new Brand { Id = 2, Name = "Adidas", Image = "adidas.png" },
                new Brand { Id = 3, Name = "Puma", Image = "puma.png" }
            );
        }
        public static void SeedColors()
        {
            _builder!.Entity<Color>().HasData(
                new Color { Id = 1, Name = "Red", Image = "red.png" },
                new Color { Id = 2, Name = "Blue", Image = "blue.png" },
                new Color { Id = 3, Name = "Black", Image = "black.png" }
            );
        }
        public static void SeedItemTypes()
        {
            _builder!.Entity<ItemType>().HasData(
                new ItemType { Id = 1, Name = "Shirts", Image = "clothing.png", ItemTypeId = null },
                new ItemType { Id = 2, Name = "Shoes", Image = "shoes.png", ItemTypeId = null },
                new ItemType { Id = 3, Name = "Short Sleeve Shirts", Image = "tshirts.png", ItemTypeId = 1 }, // child of Clothing
                new ItemType { Id = 4, Name = "Sporting Shoes", Image = "sneakers.png", ItemTypeId = 2 } // child of Shoes
            );
        }
        public static void SeedSizes()
        {
            _builder!.Entity<Size>().HasData(
                new Size { Id = 1, Name = "Small", Image = "s.png" },
                new Size { Id = 2, Name = "Medium", Image = "m.png" },
                new Size { Id = 3, Name = "Large", Image = "l.png" }
            );
        }
        public static void SeedTargetAudiences()
        {
            _builder!.Entity<TargetAudience>().HasData(
                new TargetAudience { Id = 1, Name = "Men", Image = "men.png" },
                new TargetAudience { Id = 2, Name = "Women", Image = "women.png" },
                new TargetAudience { Id = 3, Name = "Kids", Image = "kids.png" }
            );
        }
        public static void SeedUsers()
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var admin = new ApplicationUser
            {
                Id = "ADMIN-0001",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "Admin@system.com",
                NormalizedEmail = "ADMIN@SYSTEM.COM",
                EmailConfirmed = true,
                SecurityStamp = "c67d3f6a-9df2-4c2b-9d29-123456789abc",
                ConcurrencyStamp = "3b8e7c11-4f94-44b6-bd2a-23456789abcd",
                PhoneNumber = "+201000000000",
                PhoneNumberConfirmed = false,
                BranchId = null, // Admin not tied to a branch
                CreatedDate = new DateTime(2025, 1, 1),
                PasswordHash = "AQAAAAIAAYagAAAAEKJjDl/uiaaUyRTYVIUZQTZ+8Ag91FJZAO++PSmmunHQ4zeKohcN1tdI4+EBTgKGcA=="
            };


            var cashier = new ApplicationUser
            {
                Id = "CASHIER-0001",
                UserName = "Cashier",
                NormalizedUserName = "CASHIER",
                Email = "Cashier@system.com",
                NormalizedEmail = "CASHIER@SYSTEM.COM",
                EmailConfirmed = true,
                SecurityStamp = "a1234567-b89c-4def-9012-1234567890ab",
                ConcurrencyStamp = "b9876543-c21d-4fed-8765-abcdef123456",
                PhoneNumber = "+201111111111",
                PhoneNumberConfirmed = true,
                BranchId = 1,
                CreatedDate = new DateTime(2025, 1, 1),
                PasswordHash = "AQAAAAIAAYagAAAAEM1n6yL7RkI7GnXYVPh4eM6S2f/JXc1qR9mVh6Qv0I8K4OaX9O0xSck5x8uQ3+eU9w==",


            };

            var branchUser = new ApplicationUser
            {
                Id = "BRANCH-0001",
                UserName = "BranchUser",
                NormalizedUserName = "BRANCHUSER",
                Email = "BranchUser@system.com",
                NormalizedEmail = "BRANCHUSER@SYSTEM.COM",
                EmailConfirmed = true,
                SecurityStamp = "e72d5a42-5ff3-4e92-a34c-abcdef123456",
                ConcurrencyStamp = "d11a4b23-9b73-44b6-9e28-fedcba987654",
                PhoneNumber = "+201222222222",
                PhoneNumberConfirmed = true,
                BranchId = 1, // assumes branch with Id=1 exists
                CreatedDate = new DateTime(2025, 1, 1),
                PasswordHash = "AQAAAAIAAYagAAAAEKH+0zUzi5Y9I0F5VZ1+6tH8bUuQ3YzHf+8eK1J1N3mOqP2bXcW5r1y9Q1x7sU2F=="


            };
            _builder!.Entity<ApplicationUser>().HasData(admin, cashier, branchUser);
        }
        public static void SeedRoles()
        {
            _builder!.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "ROLE-Super Admin",
                    Name = "Super Admin",
                    NormalizedName = "SUPER ADMIN",
                    ConcurrencyStamp = "00000000-0000-0000-0000-00000000000"
                },
                new IdentityRole
                {
                    Id = "ROLE-ADMIN",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "11111111-1111-1111-1111-111111111111"
                },
                new IdentityRole
                {
                    Id = "ROLE-CASHIER",
                    Name = "Cashier User",
                    NormalizedName = "CASHIER USER",
                    ConcurrencyStamp = "22222222-2222-2222-2222-222222222222"
                },
                new IdentityRole
                {
                    Id = "ROLE-BRANCH",
                    Name = "Branch User",
                    NormalizedName = "BRANCH USER", // fixed (was same as cashier before)
                    ConcurrencyStamp = "33333333-3333-3333-3333-333333333333"
                }
            );


        }
        public static void SeedUserRoles()
        {
            _builder!.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "ADMIN-0001",
                    RoleId = "ROLE-ADMIN"
                },
                new IdentityUserRole<string>
                {
                    UserId = "CASHIER-0001",
                    RoleId = "ROLE-CASHIER"
                },
                new IdentityUserRole<string>
                {
                    UserId = "BRANCH-0001",
                    RoleId = "ROLE-BRANCH"
                }
            );
        }

        public static void SeedItems()
        {
            _builder!.Entity<Item>().HasData(
            new Item
            {
                Id = 1,
                Name = "Polo Tshirt",
                Barcode = "ITEM-0001",
                Image = "PoloTshirt.jpg",
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),

                BrandId = 1,
                ColorId = 1,
                ItemTypeId = 3,
                SizeId = 1,
                TargetAudienceId = 1
            },
            new Item
            {
                Id = 2,
                Name = "Zara Tshirt",
                Barcode = "ITEM-0002",
                Image = "ZaraTshirt.jpg",
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),

                BrandId = 2,
                ColorId = 2,
                ItemTypeId = 3,
                SizeId = 1,
                TargetAudienceId = 1
            });
        }
        public static void SeedBranchItem()
        {
            _builder!.Entity<BranchItem>().HasData(
             new BranchItem { BranchId = 1, ItemId = 1 },
             new BranchItem { BranchId = 1, ItemId = 2 },
             new BranchItem { BranchId = 2, ItemId = 1 },
             new BranchItem { BranchId = 2, ItemId = 2 },
             new BranchItem { BranchId = 3, ItemId = 1 },
             new BranchItem { BranchId = 3, ItemId = 2 }
            );
        }
        public static void SeedReceiveOrder()
        {
            // Seed Receive Orders
            var branchOrder = new ReceiveOrder
            {
                Id = 1,
                Code = "2_1_1",
                Date = new DateOnly(2025, 01, 01),
                Time = new TimeOnly(10, 0, 0),
                status = Status.Approved,
                TotalQuantity = 2,
                TotalAmount = 15200,
                TotalTaxesRate = 10,
                TotalTaxesAmount = 1520,
                TotalDiscountRate = 0,
                TotalDiscountAmount = 0,
                GrandTotal = 16720,
                RoundedGrandTotal = 16720,

                ApplicationUserId = "BRANCH-0001", // seeded earlier
                BranchId = 1,
                SupplierId = 4
            };

            var adminOrder = new ReceiveOrder
            {
                Id = 2,
                Code = "2_1_2",
                Date = new DateOnly(2025, 03, 01),
                Time = new TimeOnly(10, 0, 0),
                status = Status.Approved,

                TotalQuantity = 1,
                TotalAmount = 15000,
                TotalTaxesRate = 5,
                TotalTaxesAmount = 750,
                TotalDiscountRate = 0,
                TotalDiscountAmount = 0,
                GrandTotal = 15750,
                RoundedGrandTotal = 15750,

                ApplicationUserId = "ADMIN-0001", // seeded earlier
                BranchId = 1,
                SupplierId = 4
            };

            _builder!.Entity<ReceiveOrder>().HasData(branchOrder, adminOrder);


            // Seed Operation Items
            _builder!.Entity<OperationItem>().HasData(
                new OperationItem
                {
                    Id = 1,
                    OperationId = 1, // Branch Manager’s order
                    ItemId = 1,
                    Quantity = 1,
                    BuyingPrice = 15000,
                    SellingPrice = 0, // not used in receive order
                    TotalPrice = 15000
                },
                new OperationItem
                {
                    Id = 2,
                    OperationId = 1,
                    ItemId = 2,
                    Quantity = 1,
                    BuyingPrice = 200,
                    SellingPrice = 0,
                    TotalPrice = 200
                },
                new OperationItem
                {
                    Id = 3,
                    OperationId = 2, // Admin’s order
                    ItemId = 1, // Laptop
                    Quantity = 1,
                    BuyingPrice = 15000,
                    SellingPrice = 0,
                    TotalPrice = 15000
                }
            );
        }
        public static void SeedSalesInvoices()
        {
            var invoiceOne = new SalesInvoice
            {
                Id = 3,
                Code = "1_1_3",
                Date = new DateOnly(2025, 01, 01),
                Time = new TimeOnly(10, 0, 0),
                status = Status.Approved,
                TotalQuantity = 50,
                TotalAmount = 5000,
                TotalDiscountRate = 2,
                TotalDiscountAmount = 50,
                GrandTotal = 4950,
                RoundedGrandTotal = 4950,
                PaidCash = 5000,
                Change = 50,

                ApplicationUserId = "BRANCH-0001", // seeded earlier
                BranchId = 1,
                RetailCustomerId = 1
            };

            var invoiceTwo = new SalesInvoice
            {
                Id = 4,
                Code = "1_1_4",
                Date = new DateOnly(2025, 01, 01),
                Time = new TimeOnly(10, 0, 0),
                status = Status.Approved,
                TotalQuantity = 20,
                TotalAmount = 2000,
                TotalDiscountRate = 0,
                TotalDiscountAmount = 0,
                GrandTotal = 2000,
                RoundedGrandTotal = 2000,
                PaidCash = 2000,
                Change = 0,

                ApplicationUserId = "CASHIER-0001", // seeded earlier
                BranchId = 1,
                RetailCustomerId = 2
            };
            _builder!.Entity<SalesInvoice>().HasData(invoiceOne, invoiceTwo);

            // Seed Operation Items
            _builder!.Entity<OperationItem>().HasData(
                new OperationItem
                {
                    Id = 4,
                    OperationId = 3,
                    ItemId = 1,
                    Quantity = 50,
                    BuyingPrice = 0,
                    SellingPrice = 100,
                    TotalPrice = 5000
                },
                new OperationItem
                {
                    Id = 5,
                    OperationId = 4,
                    ItemId = 1,
                    Quantity = 10,
                    BuyingPrice = 0,
                    SellingPrice = 100,
                    TotalPrice = 1000
                },
                new OperationItem
                {
                    Id = 6,
                    OperationId = 4,
                    ItemId = 2,
                    Quantity = 10,
                    BuyingPrice = 0,
                    SellingPrice = 100,
                    TotalPrice = 1000
                });

            // Seed Bridge Table - Discount SalesInvoices
            _builder!.Entity<DiscountSalesInvoice>().HasData(
                new DiscountSalesInvoice
                {
                    OperationId = 3,
                    DiscountId = 3
                }
                );

            // Seed Bridge Table - BranchItem SalesInvoices
            _builder!.Entity<BranchItemSalesInvoice>().HasData(
                new BranchItemSalesInvoice
                {
                    OperationId = 3,
                    ItemId = 1,
                    BranchId = 1
                },
                new BranchItemSalesInvoice
                {
                    OperationId = 4,
                    ItemId = 1,
                    BranchId = 2
                },
                new BranchItemSalesInvoice
                {
                    OperationId = 4,
                    ItemId = 2,
                    BranchId = 2
                }
                );
        }
    }
}