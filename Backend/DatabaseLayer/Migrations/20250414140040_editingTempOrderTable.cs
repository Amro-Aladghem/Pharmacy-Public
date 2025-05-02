using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class editingTempOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempOrderRequests_Pharmacies_PharmacyId",
                table: "TempOrderRequests");

            migrationBuilder.DropIndex(
                name: "IX_TempOrderRequests_PharmacyId",
                table: "TempOrderRequests");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "TempOrderRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PharmacyId",
                table: "TempOrderRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TempOrderRequests_PharmacyId",
                table: "TempOrderRequests",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TempOrderRequests_Pharmacies_PharmacyId",
                table: "TempOrderRequests",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "PharmacyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
