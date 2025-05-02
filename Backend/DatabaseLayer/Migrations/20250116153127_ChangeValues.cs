using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DeliveryFees",
                columns: new[] { "Id", "Fees", "MaxDistanceKm", "MinDistanceKm" },
                values: new object[,]
                {
                    { 4, 1.00m, 3.00m, 0.0m },
                    { 5, 2.00m, 20.00m, 3.10m },
                    { 6, 3.00m, 35.00m, 20.10m },
                    { 7, 4.00m, 50.00m, 35.10m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DeliveryFees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DeliveryFees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DeliveryFees",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "DeliveryFees",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
