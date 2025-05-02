using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class addNewUserTypePreRegistered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiredDate",
                table: "Tokens",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "TypeId", "UserTypeName" },
                values: new object[] { 5, "Pre_Registered" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "TypeId",
                keyValue: 5);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ExpiredDate",
                table: "Tokens",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
