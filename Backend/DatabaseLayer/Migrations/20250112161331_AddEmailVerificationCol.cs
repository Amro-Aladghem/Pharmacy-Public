using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailVerificationCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailVerifications",
                columns: table => new
                {
                    VerificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    RefferenceId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateOfCreated = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "CONVERT(DATE,GETDATE())"),
                    TimeOfCreated = table.Column<TimeOnly>(type: "TIME(0)", nullable: false, defaultValueSql: "CONVERT(TIME,GETDATE())"),
                    TimeOfExpired = table.Column<TimeOnly>(type: "TIME(0)", nullable: false, computedColumnSql: "DATEADD(MINUTE,3,[TimeOfCreated])", stored: true),
                    AttemptCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerifications", x => x.VerificationId);
                    table.ForeignKey(
                        name: "FK_EmailVerifications_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerifications_UserTypeId",
                table: "EmailVerifications",
                column: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVerifications");
        }
    }
}
