using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCartItemColName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_PhPramacyProducts_PhPharmacyId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "PhPharmacyId",
                table: "CartItems",
                newName: "PhProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_PhPharmacyId",
                table: "CartItems",
                newName: "IX_CartItems_PhProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_PhPramacyProducts_PhProductId",
                table: "CartItems",
                column: "PhProductId",
                principalTable: "PhPramacyProducts",
                principalColumn: "PhProductId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_PhPramacyProducts_PhProductId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "PhProductId",
                table: "CartItems",
                newName: "PhPharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_PhProductId",
                table: "CartItems",
                newName: "IX_CartItems_PhPharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_PhPramacyProducts_PhPharmacyId",
                table: "CartItems",
                column: "PhPharmacyId",
                principalTable: "PhPramacyProducts",
                principalColumn: "PhProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
