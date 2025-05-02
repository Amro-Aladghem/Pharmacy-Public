using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class addPaymentMethodeIdRowInOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodeId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentMethodeId",
                table: "Orders",
                column: "PaymentMethodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentMethodes_PaymentMethodeId",
                table: "Orders",
                column: "PaymentMethodeId",
                principalTable: "PaymentMethodes",
                principalColumn: "MethodeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentMethodes_PaymentMethodeId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentMethodeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethodeId",
                table: "Orders");
        }
    }
}
