using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CurrentUses = table.Column<int>(type: "int", nullable: false),
                    MaximumUses = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTypes_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    partnerType = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Permissions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TargetAudiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetAudiences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    ItemTypeId = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: false),
                    TargetAudienceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_TargetAudiences_TargetAudienceId",
                        column: x => x.TargetAudienceId,
                        principalTable: "TargetAudiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserOTP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OTPNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserOTP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserOTP_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<double>(type: "float", nullable: false),
                    TotalTaxesRate = table.Column<int>(type: "int", nullable: true),
                    TotalTaxesAmount = table.Column<double>(type: "float", nullable: true),
                    TotalDiscountRate = table.Column<int>(type: "int", nullable: true),
                    TotalDiscountAmount = table.Column<double>(type: "float", nullable: true),
                    GrandTotal = table.Column<double>(type: "float", nullable: false),
                    RoundedGrandTotal = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userLoginHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userLoginHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userLoginHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchItems",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DiscountRate = table.Column<int>(type: "int", nullable: true),
                    BuyingPriceAvg = table.Column<double>(type: "float", nullable: false),
                    LastBuyingPrice = table.Column<double>(type: "float", nullable: false),
                    SellingPrice = table.Column<double>(type: "float", nullable: true),
                    RestockThreshold = table.Column<int>(type: "int", nullable: false),
                    OutDatedInMonths = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchItems", x => new { x.BranchId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_BranchItems_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<double>(type: "float", nullable: false),
                    BuyingPrice = table.Column<double>(type: "float", nullable: false),
                    DiscountRate = table.Column<int>(type: "int", nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationItems_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiveOrders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveOrders_Operations_Id",
                        column: x => x.Id,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveOrders_Partners_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    RetailCustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesInvoices_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesInvoices_Operations_Id",
                        column: x => x.Id,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesInvoices_Partners_RetailCustomerId",
                        column: x => x.RetailCustomerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FromBranchId = table.Column<int>(type: "int", nullable: false),
                    ToBranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_Branches_FromBranchId",
                        column: x => x.FromBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Branches_ToBranchId",
                        column: x => x.ToBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Operations_Id",
                        column: x => x.Id,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxReceiveOrders",
                columns: table => new
                {
                    TaxId = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxReceiveOrders", x => new { x.TaxId, x.OperationId });
                    table.ForeignKey(
                        name: "FK_TaxReceiveOrders_ReceiveOrders_OperationId",
                        column: x => x.OperationId,
                        principalTable: "ReceiveOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxReceiveOrders_Taxes_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Taxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchItemSalesInvoices",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchItemSalesInvoices", x => new { x.BranchId, x.ItemId, x.OperationId });
                    table.ForeignKey(
                        name: "FK_BranchItemSalesInvoices_BranchItems_BranchId_ItemId",
                        columns: x => new { x.BranchId, x.ItemId },
                        principalTable: "BranchItems",
                        principalColumns: new[] { "BranchId", "ItemId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchItemSalesInvoices_SalesInvoices_OperationId",
                        column: x => x.OperationId,
                        principalTable: "SalesInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountSalesInvoices",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountSalesInvoices", x => new { x.DiscountId, x.OperationId });
                    table.ForeignKey(
                        name: "FK_DiscountSalesInvoices_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountSalesInvoices_SalesInvoices_OperationId",
                        column: x => x.OperationId,
                        principalTable: "SalesInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ROLE-ADMIN", "11111111-1111-1111-1111-111111111111", "Admin", "ADMIN" },
                    { "ROLE-BRANCH", "33333333-3333-3333-3333-333333333333", "Branch User", "BRANCH USER" },
                    { "ROLE-CASHIER", "22222222-2222-2222-2222-222222222222", "Cashier User", "CASHIER USER" },
                    { "ROLE-Super Admin", "00000000-0000-0000-0000-00000000000", "Super Admin", "SUPER ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BranchId", "ConcurrencyStamp", "CreatedDate", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ADMIN-0001", 0, null, "3b8e7c11-4f94-44b6-bd2a-23456789abcd", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin@system.com", true, false, null, "ADMIN@SYSTEM.COM", "ADMIN", "AQAAAAIAAYagAAAAEKJjDl/uiaaUyRTYVIUZQTZ+8Ag91FJZAO++PSmmunHQ4zeKohcN1tdI4+EBTgKGcA==", "+201000000000", false, "c67d3f6a-9df2-4c2b-9d29-123456789abc", false, "Admin" });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "Address", "CreatedDate", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "123 Main Street, City Center", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main Branch", "+1234567890" },
                    { 2, "456 East Street, East Town", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "East Side Branch", "+1987654321" },
                    { 3, "789 West Avenue, Westside", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "West End Branch", "+1123456789" }
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Image", "Name" },
                values: new object[,]
                {
                    { 1, "nike.png", "Nike" },
                    { 2, "adidas.png", "Adidas" },
                    { 3, "puma.png", "Puma" }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "Image", "Name" },
                values: new object[,]
                {
                    { 1, "red.png", "Red" },
                    { 2, "blue.png", "Blue" },
                    { 3, "black.png", "Black" }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "CurrentUses", "ExpirationDate", "IsActive", "MaximumUses", "Name", "Rate" },
                values: new object[,]
                {
                    { 1, 0, new DateOnly(2028, 1, 1), true, 100, "New Year Offer", 15 },
                    { 2, 0, new DateOnly(2025, 11, 30), true, 500, "Black Friday", 25 },
                    { 3, 0, null, true, null, "Loyalty Discount", 10 }
                });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "Id", "Image", "ItemTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "clothing.png", null, "Clothing" },
                    { 2, "shoes.png", null, "Shoes" }
                });

            migrationBuilder.InsertData(
                table: "Partners",
                columns: new[] { "Id", "Email", "Name", "PhoneNumber", "partnerType" },
                values: new object[,]
                {
                    { 1, "contact@abc.com", "ABC Suppliers", "+201001112233", 1 },
                    { 2, "john.doe@example.com", "John Doe", "+201112223344", 2 },
                    { 3, "sales@xyzretail.com", "XYZ Retail", "+201223344556", 2 }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "EnglishName", "Name", "ParentId" },
                values: new object[] { 1, "System", "System", null });

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "Image", "Name" },
                values: new object[,]
                {
                    { 1, "s.png", "Small" },
                    { 2, "m.png", "Medium" },
                    { 3, "l.png", "Large" }
                });

            migrationBuilder.InsertData(
                table: "TargetAudiences",
                columns: new[] { "Id", "Image", "Name" },
                values: new object[,]
                {
                    { 1, "men.png", "Men" },
                    { 2, "women.png", "Women" },
                    { 3, "kids.png", "Kids" }
                });

            migrationBuilder.InsertData(
                table: "Taxes",
                columns: new[] { "Id", "Name", "Rate" },
                values: new object[,]
                {
                    { 1, "VAT", 14 },
                    { 2, "Service Tax", 10 },
                    { 3, "Luxury Tax", 5 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ROLE-ADMIN", "ADMIN-0001" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BranchId", "ConcurrencyStamp", "CreatedDate", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "BRANCH-0001", 0, 1, "d11a4b23-9b73-44b6-9e28-fedcba987654", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BranchUser@system.com", true, false, null, "BRANCHUSER@SYSTEM.COM", "BRANCHUSER", "AQAAAAIAAYagAAAAEKH+0zUzi5Y9I0F5VZ1+6tH8bUuQ3YzHf+8eK1J1N3mOqP2bXcW5r1y9Q1x7sU2F==", "+201222222222", true, "e72d5a42-5ff3-4e92-a34c-abcdef123456", false, "BranchUser" },
                    { "CASHIER-0001", 0, 1, "b9876543-c21d-4fed-8765-abcdef123456", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cashier@system.com", true, false, null, "CASHIER@SYSTEM.COM", "CASHIER", "AQAAAAIAAYagAAAAEM1n6yL7RkI7GnXYVPh4eM6S2f/JXc1qR9mVh6Qv0I8K4OaX9O0xSck5x8uQ3+eU9w==", "+201111111111", true, "a1234567-b89c-4def-9012-1234567890ab", false, "Cashier" }
                });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "Id", "Image", "ItemTypeId", "Name" },
                values: new object[,]
                {
                    { 3, "tshirts.png", 1, "T-Shirts" },
                    { 4, "sneakers.png", 2, "Sneakers" }
                });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "ApplicationUserId", "Code", "Date", "GrandTotal", "RoundedGrandTotal", "Time", "TotalAmount", "TotalDiscountAmount", "TotalDiscountRate", "TotalQuantity", "TotalTaxesAmount", "TotalTaxesRate", "status" },
                values: new object[] { 2, "ADMIN-0001", "2_1_2", new DateOnly(2025, 3, 1), 15750.0, 15750, new TimeOnly(10, 0, 0), 15000.0, 0.0, 0, 1, 750.0, 5, 1 });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "EnglishName", "Name", "ParentId" },
                values: new object[,]
                {
                    { 10, "Branch", "Branch", 1 },
                    { 11, "Item", "Item", 1 },
                    { 12, "Administrative", "Administrative", 1 },
                    { 13, "Sales", "Sales", 1 },
                    { 14, "Dashboard", "Dashboard", 1 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "ROLE-BRANCH", "BRANCH-0001" },
                    { "ROLE-CASHIER", "CASHIER-0001" }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Barcode", "BrandId", "ColorId", "CreatedDate", "Image", "ItemTypeId", "Name", "SizeId", "TargetAudienceId" },
                values: new object[,]
                {
                    { 1, "ITEM-0001", 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PoloTshirt.jpg", 3, "Polo Tshirt", 1, 1 },
                    { 2, "ITEM-0002", 2, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ZaraTshirt.jpg", 3, "Zara Tshirt", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "ApplicationUserId", "Code", "Date", "GrandTotal", "RoundedGrandTotal", "Time", "TotalAmount", "TotalDiscountAmount", "TotalDiscountRate", "TotalQuantity", "TotalTaxesAmount", "TotalTaxesRate", "status" },
                values: new object[] { 1, "BRANCH-0001", "2_1_1", new DateOnly(2025, 1, 1), 16720.0, 16720, new TimeOnly(10, 0, 0), 15200.0, 0.0, 0, 2, 1520.0, 10, 1 });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "EnglishName", "Name", "ParentId" },
                values: new object[,]
                {
                    { 100, "Stock", "Stock", 10 },
                    { 120, "Receive Order", "ReceiveOrder", 10 },
                    { 140, "Clothing Item", "ClothingItem", 11 },
                    { 160, "Color", "Color", 11 },
                    { 180, "Size", "Size", 11 },
                    { 200, "Item Type", "ItemType", 11 },
                    { 220, "Target Audience", "TargetAudience", 11 },
                    { 240, "Brand", "Brand", 11 },
                    { 260, "Partner", "Partner", 12 },
                    { 280, "User", "User", 12 },
                    { 300, "Role", "Role", 12 },
                    { 340, "Tax", "Tax", 12 },
                    { 360, "Discount", "Discount", 12 },
                    { 380, "User Login History", "UserLoginHistory", 12 },
                    { 400, "POS", "POS", 13 },
                    { 420, "Sales Invoice", "SalesInvoice", 13 },
                    { 440, "View Dashboard", "Dashboard.View", 14 }
                });

            migrationBuilder.InsertData(
                table: "ReceiveOrders",
                columns: new[] { "Id", "BranchId", "SupplierId" },
                values: new object[] { 2, 1, 1 });

            migrationBuilder.InsertData(
                table: "BranchItems",
                columns: new[] { "BranchId", "ItemId", "BuyingPriceAvg", "DiscountRate", "LastBuyingPrice", "OutDatedInMonths", "Quantity", "RestockThreshold", "SellingPrice" },
                values: new object[,]
                {
                    { 1, 1, 0.0, null, 0.0, 0, 0, 0, null },
                    { 1, 2, 0.0, null, 0.0, 0, 0, 0, null },
                    { 2, 1, 0.0, null, 0.0, 0, 0, 0, null },
                    { 2, 2, 0.0, null, 0.0, 0, 0, 0, null },
                    { 3, 1, 0.0, null, 0.0, 0, 0, 0, null },
                    { 3, 2, 0.0, null, 0.0, 0, 0, 0, null }
                });

            migrationBuilder.InsertData(
                table: "OperationItems",
                columns: new[] { "Id", "BuyingPrice", "DiscountRate", "ItemId", "OperationId", "Quantity", "SellingPrice", "TotalPrice" },
                values: new object[,]
                {
                    { 1, 15000.0, null, 1, 1, 1, 0.0, 15000.0 },
                    { 2, 200.0, null, 2, 1, 1, 0.0, 200.0 },
                    { 3, 15000.0, null, 1, 2, 1, 0.0, 15000.0 }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "EnglishName", "Name", "ParentId" },
                values: new object[,]
                {
                    { 101, "View Stock", "Stock.View", 100 },
                    { 102, "Add Stock", "Stock.Add", 100 },
                    { 103, "Edit Stock", "Stock.Edit", 100 },
                    { 104, "Delete Stock", "Stock.Delete", 100 },
                    { 121, "View Receive Order", "ReceiveOrder.View", 120 },
                    { 122, "Add Receive Order", "ReceiveOrder.Add", 120 },
                    { 123, "Edit Receive Order", "ReceiveOrder.Edit", 120 },
                    { 124, "Delete Receive Order", "ReceiveOrder.Delete", 120 },
                    { 125, "Confirm Receive Order", "ReceiveOrder.Confirm", 120 },
                    { 141, "View ClothingItem", "ClothingItem.View", 140 },
                    { 142, "Add ClothingItem", "ClothingItem.Add", 140 },
                    { 143, "Edit ClothingItem", "ClothingItem.Edit", 140 },
                    { 144, "Delete ClothingItem", "ClothingItem.Delete", 140 },
                    { 145, "Add Data To A Branch", "ClothingItem.BranchItem", 140 },
                    { 161, "View Color", "Color.View", 160 },
                    { 162, "Add Color", "Color.Add", 160 },
                    { 163, "Edit Color", "Color.Edit", 160 },
                    { 164, "Delete Color", "Color.Delete", 160 },
                    { 181, "View Size", "Size.View", 180 },
                    { 182, "Add Size", "Size.Add", 180 },
                    { 183, "Edit Size", "Size.Edit", 180 },
                    { 184, "Delete Size", "Size.Delete", 180 },
                    { 201, "View Item Type", "ItemType.View", 200 },
                    { 202, "Add Item Type", "ItemType.Add", 200 },
                    { 203, "Edit Item Type", "ItemType.Edit", 200 },
                    { 204, "Delete Item Type", "ItemType.Delete", 200 },
                    { 221, "View Target Audience", "TargetAudience.View", 220 },
                    { 222, "Add Target Audience", "TargetAudience.Add", 220 },
                    { 223, "Edit Target Audience", "TargetAudience.Edit", 220 },
                    { 224, "Delete Target Audience", "TargetAudience.Delete", 220 },
                    { 241, "View Brand", "Brand.View", 240 },
                    { 242, "Add Brand", "Brand.Add", 240 },
                    { 243, "Edit Brand", "Brand.Edit", 240 },
                    { 244, "Delete Brand", "Brand.Delete", 240 },
                    { 261, "View Partner", "Partner.View", 260 },
                    { 262, "Add Partner", "Partner.Add", 260 },
                    { 263, "Edit Partner", "Partner.Edit", 260 },
                    { 264, "Delete Partner", "Partner.Delete", 260 },
                    { 281, "View User", "User.View", 280 },
                    { 282, "Add User", "User.Add", 280 },
                    { 283, "Edit User", "User.Edit", 280 },
                    { 284, "Delete User", "User.Delete", 280 },
                    { 301, "View Role", "Role.View", 300 },
                    { 302, "Add Role", "Role.Add", 300 },
                    { 303, "Edit Role", "Role.Edit", 300 },
                    { 304, "Delete Role", "Role.Delete", 300 },
                    { 341, "View Tax", "Tax.View", 340 },
                    { 342, "Add Tax", "Tax.Add", 340 },
                    { 343, "Edit Tax", "Tax.Edit", 340 },
                    { 344, "Delete Tax", "Tax.Delete", 340 },
                    { 361, "View Discount", "Discount.View", 360 },
                    { 362, "Add Discount", "Discount.Add", 360 },
                    { 363, "Edit Discount", "Discount.Edit", 360 },
                    { 364, "Delete Discount", "Discount.Delete", 360 },
                    { 381, "View UserLoginHistory", "UserLoginHistory.View", 380 },
                    { 421, "View Sales Invoice", "SalesInvoice.View", 420 },
                    { 422, "Print Sales Invoice", "SalesInvoice.Print", 420 }
                });

            migrationBuilder.InsertData(
                table: "ReceiveOrders",
                columns: new[] { "Id", "BranchId", "SupplierId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserOTP_ApplicationUserId",
                table: "ApplicationUserOTP",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BranchId",
                table: "AspNetUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BranchItems_ItemId",
                table: "BranchItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchItemSalesInvoices_OperationId",
                table: "BranchItemSalesInvoices",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_Name",
                table: "Discounts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscountSalesInvoices_OperationId",
                table: "DiscountSalesInvoices",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Barcode",
                table: "Items",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_BrandId",
                table: "Items",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ColorId",
                table: "Items",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeId",
                table: "Items",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                table: "Items",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_SizeId",
                table: "Items",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_TargetAudienceId",
                table: "Items",
                column: "TargetAudienceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_ItemTypeId_Name",
                table: "ItemTypes",
                columns: new[] { "ItemTypeId", "Name" },
                unique: true,
                filter: "[ItemTypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_Name",
                table: "ItemTypes",
                column: "Name",
                unique: true,
                filter: "[ItemTypeId] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OperationItems_ItemId",
                table: "OperationItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationItems_OperationId",
                table: "OperationItems",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ApplicationUserId",
                table: "Operations",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_Email",
                table: "Partners",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_Name",
                table: "Partners",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ParentId",
                table: "Permissions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveOrders_BranchId",
                table: "ReceiveOrders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveOrders_SupplierId",
                table: "ReceiveOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_BranchId",
                table: "SalesInvoices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_RetailCustomerId",
                table: "SalesInvoices",
                column: "RetailCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_Name",
                table: "Taxes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxReceiveOrders_OperationId",
                table: "TaxReceiveOrders",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_FromBranchId",
                table: "Transfers",
                column: "FromBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ToBranchId",
                table: "Transfers",
                column: "ToBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_userLoginHistories_UserId",
                table: "userLoginHistories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserOTP");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BranchItemSalesInvoices");

            migrationBuilder.DropTable(
                name: "DiscountSalesInvoices");

            migrationBuilder.DropTable(
                name: "OperationItems");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "TaxReceiveOrders");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "userLoginHistories");

            migrationBuilder.DropTable(
                name: "BranchItems");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "SalesInvoices");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "ReceiveOrders");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropTable(
                name: "TargetAudiences");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
