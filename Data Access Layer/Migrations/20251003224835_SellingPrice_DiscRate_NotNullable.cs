using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class SellingPrice_DiscRate_NotNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "BranchItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiscountRate",
                table: "BranchItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 1, 1 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { 0, 0.0 });

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 1, 2 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { 0, 0.0 });

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 2, 1 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { 0, 0.0 });

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 2, 2 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { 0, 0.0 });

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 3, 1 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { 0, 0.0 });

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 3, 2 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { 0, 0.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "BranchItems",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountRate",
                table: "BranchItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 1, 1 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 1, 2 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 2, 1 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 2, 2 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 3, 1 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BranchItems",
                keyColumns: new[] { "BranchId", "ItemId" },
                keyValues: new object[] { 3, 2 },
                columns: new[] { "DiscountRate", "SellingPrice" },
                values: new object[] { null, null });
        }
    }
}
