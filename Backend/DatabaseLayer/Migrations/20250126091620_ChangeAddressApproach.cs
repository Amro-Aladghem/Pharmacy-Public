using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAddressApproach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingPayments_RequestMeeting_RequestId",
                table: "MeetingPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_RequestMeeting_RequestId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestMeeting_Customers_CustomerId",
                table: "RequestMeeting");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestMeeting_Pharmacies_PharmacyId",
                table: "RequestMeeting");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestMeeting_RequestStatus_RequestStatusId",
                table: "RequestMeeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestMeeting",
                table: "RequestMeeting");

            migrationBuilder.DropColumn(
                name: "AddressCode",
                table: "Pharmacies");

            migrationBuilder.RenameTable(
                name: "RequestMeeting",
                newName: "RequestMeetings");

            migrationBuilder.RenameIndex(
                name: "IX_RequestMeeting_RequestStatusId",
                table: "RequestMeetings",
                newName: "IX_RequestMeetings_RequestStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestMeeting_PharmacyId",
                table: "RequestMeetings",
                newName: "IX_RequestMeetings_PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestMeeting_CustomerId",
                table: "RequestMeetings",
                newName: "IX_RequestMeetings_CustomerId");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Pharmacies",
                type: "decimal(10,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Pharmacies",
                type: "decimal(10,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestMeetings",
                table: "RequestMeetings",
                column: "RequestId");

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "TypeId", "UserTypeName" },
                values: new object[] { 3, "Manager" });

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingPayments_RequestMeetings_RequestId",
                table: "MeetingPayments",
                column: "RequestId",
                principalTable: "RequestMeetings",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_RequestMeetings_RequestId",
                table: "Meetings",
                column: "RequestId",
                principalTable: "RequestMeetings",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMeetings_Customers_CustomerId",
                table: "RequestMeetings",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CutomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMeetings_Pharmacies_PharmacyId",
                table: "RequestMeetings",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "PharmacyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMeetings_RequestStatus_RequestStatusId",
                table: "RequestMeetings",
                column: "RequestStatusId",
                principalTable: "RequestStatus",
                principalColumn: "ReqStatusId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingPayments_RequestMeetings_RequestId",
                table: "MeetingPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_RequestMeetings_RequestId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestMeetings_Customers_CustomerId",
                table: "RequestMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestMeetings_Pharmacies_PharmacyId",
                table: "RequestMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestMeetings_RequestStatus_RequestStatusId",
                table: "RequestMeetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestMeetings",
                table: "RequestMeetings");

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "TypeId",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Pharmacies");

            migrationBuilder.RenameTable(
                name: "RequestMeetings",
                newName: "RequestMeeting");

            migrationBuilder.RenameIndex(
                name: "IX_RequestMeetings_RequestStatusId",
                table: "RequestMeeting",
                newName: "IX_RequestMeeting_RequestStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestMeetings_PharmacyId",
                table: "RequestMeeting",
                newName: "IX_RequestMeeting_PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestMeetings_CustomerId",
                table: "RequestMeeting",
                newName: "IX_RequestMeeting_CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "AddressCode",
                table: "Pharmacies",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestMeeting",
                table: "RequestMeeting",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingPayments_RequestMeeting_RequestId",
                table: "MeetingPayments",
                column: "RequestId",
                principalTable: "RequestMeeting",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_RequestMeeting_RequestId",
                table: "Meetings",
                column: "RequestId",
                principalTable: "RequestMeeting",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMeeting_Customers_CustomerId",
                table: "RequestMeeting",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CutomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMeeting_Pharmacies_PharmacyId",
                table: "RequestMeeting",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "PharmacyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMeeting_RequestStatus_RequestStatusId",
                table: "RequestMeeting",
                column: "RequestStatusId",
                principalTable: "RequestStatus",
                principalColumn: "ReqStatusId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
