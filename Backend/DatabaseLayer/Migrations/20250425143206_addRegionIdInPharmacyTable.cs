using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class addRegionIdInPharmacyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Pharmacies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_RegionId",
                table: "Pharmacies",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmacies_Regions_RegionId",
                table: "Pharmacies",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "RegionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pharmacies_Regions_RegionId",
                table: "Pharmacies");

            migrationBuilder.DropIndex(
                name: "IX_Pharmacies_RegionId",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Pharmacies");
        }
    }
}
