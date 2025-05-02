using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddArabicNameToStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReqStatusArabic",
                table: "RequestStatus",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameArabic",
                table: "RefundStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StatusNameArabic",
                table: "OrderStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: 1,
                column: "StatusNameArabic",
                value: "تم الأرسال بنجاح");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: 2,
                column: "StatusNameArabic",
                value: "انتظار");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: 3,
                column: "StatusNameArabic",
                value: "يتم التحضير");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: 4,
                column: "StatusNameArabic",
                value: "توصيل");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: 5,
                column: "StatusNameArabic",
                value: "ملغاة");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: 6,
                column: "StatusNameArabic",
                value: "مرفوض");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: 7,
                column: "StatusNameArabic",
                value: "منتهي");

            migrationBuilder.UpdateData(
                table: "RefundStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "NameArabic",
                value: "تم الأرسال بنجاح");

            migrationBuilder.UpdateData(
                table: "RefundStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "NameArabic",
                value: "انتظار");

            migrationBuilder.UpdateData(
                table: "RefundStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "NameArabic",
                value: "مقبولة");

            migrationBuilder.UpdateData(
                table: "RefundStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "NameArabic",
                value: "مرفوضة");

            migrationBuilder.UpdateData(
                table: "RequestStatus",
                keyColumn: "ReqStatusId",
                keyValue: 1,
                column: "ReqStatusArabic",
                value: "تم الأرسال بنجاح");

            migrationBuilder.UpdateData(
                table: "RequestStatus",
                keyColumn: "ReqStatusId",
                keyValue: 2,
                column: "ReqStatusArabic",
                value: "انتظار");

            migrationBuilder.UpdateData(
                table: "RequestStatus",
                keyColumn: "ReqStatusId",
                keyValue: 3,
                column: "ReqStatusArabic",
                value: "مقبولة");

            migrationBuilder.UpdateData(
                table: "RequestStatus",
                keyColumn: "ReqStatusId",
                keyValue: 4,
                column: "ReqStatusArabic",
                value: "مرفوضة");

            migrationBuilder.UpdateData(
                table: "RequestStatus",
                keyColumn: "ReqStatusId",
                keyValue: 5,
                column: "ReqStatusArabic",
                value: "ملغاة");

            migrationBuilder.UpdateData(
                table: "RequestStatus",
                keyColumn: "ReqStatusId",
                keyValue: 6,
                column: "ReqStatusArabic",
                value: "منتهية");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReqStatusArabic",
                table: "RequestStatus");

            migrationBuilder.DropColumn(
                name: "NameArabic",
                table: "RefundStatuses");

            migrationBuilder.DropColumn(
                name: "StatusNameArabic",
                table: "OrderStatuses");
        }
    }
}
