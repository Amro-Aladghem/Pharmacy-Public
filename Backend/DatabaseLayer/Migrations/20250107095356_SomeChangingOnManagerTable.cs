using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class SomeChangingOnManagerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PharmacyId",
                table: "Managers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_PharmacyId",
                table: "Managers",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Pharmacies_PharmacyId",
                table: "Managers",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "PharmacyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Pharmacies_PharmacyId",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_PharmacyId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "Managers");
        }
    }
}
