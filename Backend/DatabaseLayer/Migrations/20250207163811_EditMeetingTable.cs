using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class EditMeetingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Admins_JoindAdminId",
                table: "Meetings");

            migrationBuilder.RenameColumn(
                name: "JoindAdminId",
                table: "Meetings",
                newName: "UserTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Meetings_JoindAdminId",
                table: "Meetings",
                newName: "IX_Meetings_UserTypeId");

           
            migrationBuilder.AddColumn<int>(
                name: "RefferenceId",
                table: "Meetings",
                type: "int",
                nullable: false,
                defaultValue: 0);



            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_UserTypes_UserTypeId",
                table: "Meetings",
                column: "UserTypeId",
                principalTable: "UserTypes",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Admins_AdminId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_UserTypes_UserTypeId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_AdminId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "RefferenceId",
                table: "Meetings");

            migrationBuilder.RenameColumn(
                name: "UserTypeId",
                table: "Meetings",
                newName: "JoindAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Meetings_UserTypeId",
                table: "Meetings",
                newName: "IX_Meetings_JoindAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Admins_JoindAdminId",
                table: "Meetings",
                column: "JoindAdminId",
                principalTable: "Admins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
