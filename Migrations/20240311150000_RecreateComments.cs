using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace urban_trader_be.Migrations
{
    /// <inheritdoc />
    public partial class RecreateComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e2b80d7-3ce7-4a29-9989-fdbe3c699bd7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f496c7d3-0670-4bc0-87f6-c6c3658cc9bc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "54b6c43d-5189-457e-a4e8-84e85cf0c628", null, "Admin", "ADMIN" },
                    { "95ed8c2f-bb24-43f7-bac0-19a7b5318841", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54b6c43d-5189-457e-a4e8-84e85cf0c628");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95ed8c2f-bb24-43f7-bac0-19a7b5318841");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e2b80d7-3ce7-4a29-9989-fdbe3c699bd7", null, "User", "USER" },
                    { "f496c7d3-0670-4bc0-87f6-c6c3658cc9bc", null, "Admin", "ADMIN" }
                });
        }
    }
}
