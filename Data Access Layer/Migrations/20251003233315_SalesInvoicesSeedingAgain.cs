using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class SalesInvoicesSeedingAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "ApplicationUserId", "Code", "Date", "GrandTotal", "RoundedGrandTotal", "Time", "TotalAmount", "TotalDiscountAmount", "TotalDiscountRate", "TotalQuantity", "TotalTaxesAmount", "TotalTaxesRate", "status" },
                values: new object[,]
                {
                    { 3, "BRANCH-0001", "1_1_3", new DateOnly(2025, 1, 1), 4950.0, 4950, new TimeOnly(10, 0, 0), 5000.0, 50.0, 2, 50, null, null, 1 },
                    { 4, "CASHIER-0001", "1_1_4", new DateOnly(2025, 1, 1), 2000.0, 2000, new TimeOnly(10, 0, 0), 2000.0, 0.0, 0, 20, null, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "OperationItems",
                columns: new[] { "Id", "BuyingPrice", "DiscountRate", "ItemId", "OperationId", "Quantity", "SellingPrice", "TotalPrice" },
                values: new object[,]
                {
                    { 4, 0.0, null, 1, 3, 50, 100.0, 5000.0 },
                    { 5, 0.0, null, 1, 4, 10, 100.0, 1000.0 },
                    { 6, 0.0, null, 2, 4, 10, 100.0, 1000.0 }
                });

            migrationBuilder.InsertData(
                table: "SalesInvoices",
                columns: new[] { "Id", "BranchId", "Change", "PaidCash", "RetailCustomerId" },
                values: new object[,]
                {
                    { 3, 1, 50, 5000, 1 },
                    { 4, 1, 0, 2000, 2 }
                });

            migrationBuilder.InsertData(
                table: "BranchItemSalesInvoices",
                columns: new[] { "BranchId", "ItemId", "OperationId" },
                values: new object[,]
                {
                    { 1, 1, 3 },
                    { 2, 1, 4 },
                    { 2, 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "DiscountSalesInvoices",
                columns: new[] { "DiscountId", "OperationId" },
                values: new object[] { 3, 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BranchItemSalesInvoices",
                keyColumns: new[] { "BranchId", "ItemId", "OperationId" },
                keyValues: new object[] { 1, 1, 3 });

            migrationBuilder.DeleteData(
                table: "BranchItemSalesInvoices",
                keyColumns: new[] { "BranchId", "ItemId", "OperationId" },
                keyValues: new object[] { 2, 1, 4 });

            migrationBuilder.DeleteData(
                table: "BranchItemSalesInvoices",
                keyColumns: new[] { "BranchId", "ItemId", "OperationId" },
                keyValues: new object[] { 2, 2, 4 });

            migrationBuilder.DeleteData(
                table: "DiscountSalesInvoices",
                keyColumns: new[] { "DiscountId", "OperationId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "OperationItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OperationItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OperationItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SalesInvoices",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
