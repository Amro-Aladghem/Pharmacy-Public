using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValueToCurrectCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentCurrency",
                table: "PharmacyCurrencies",
                type: "decimal(18,3)",
                nullable: false,
                defaultValue: 0.00m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentCurrency",
                table: "PharmacyCurrencies",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldDefaultValue: 0.00m);
        }
    }
}
