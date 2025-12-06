using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPartnerSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Partners",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Name", "PhoneNumber", "partnerType" },
                values: new object[] { null, "Anonymous Customer", "", 2 });

            migrationBuilder.InsertData(
                table: "Partners",
                columns: new[] { "Id", "Email", "Name", "PhoneNumber", "partnerType" },
                values: new object[] { 4, "contact@abc.com", "ABC Suppliers", "+201001112233", 1 });

            migrationBuilder.UpdateData(
                table: "ReceiveOrders",
                keyColumn: "Id",
                keyValue: 1,
                column: "SupplierId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ReceiveOrders",
                keyColumn: "Id",
                keyValue: 2,
                column: "SupplierId",
                value: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Partners",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Partners",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Name", "PhoneNumber", "partnerType" },
                values: new object[] { "contact@abc.com", "ABC Suppliers", "+201001112233", 1 });

            migrationBuilder.UpdateData(
                table: "ReceiveOrders",
                keyColumn: "Id",
                keyValue: 1,
                column: "SupplierId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ReceiveOrders",
                keyColumn: "Id",
                keyValue: 2,
                column: "SupplierId",
                value: 1);
        }
    }
}
