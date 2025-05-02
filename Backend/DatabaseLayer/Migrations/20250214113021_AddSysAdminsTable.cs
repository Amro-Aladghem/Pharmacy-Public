using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddSysAdminsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemAdmins",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PINcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LastLoggedIn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemAdmins", x => x.AdminId);
                });

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "TypeId", "UserTypeName" },
                values: new object[] { 4, "SysAmdin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemAdmins");

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "TypeId",
                keyValue: 4);
        }
    }
}
