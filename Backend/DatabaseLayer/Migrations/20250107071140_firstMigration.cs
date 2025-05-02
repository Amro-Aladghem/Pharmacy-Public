using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countrys",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countrys", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinDistanceKm = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxDistanceKm = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fees = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalTypeName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MedicalTypeNameArabic = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethodes",
                columns: table => new
                {
                    MethodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MethodeName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethodes", x => x.MethodeId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "RefundStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefundTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "RequestStatus",
                columns: table => new
                {
                    ReqStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReqStatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatus", x => x.ReqStatusId);
                });

            migrationBuilder.CreateTable(
                name: "SysProducts",
                columns: table => new
                {
                    SysProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysProducts", x => x.SysProductId);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    GovernorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.GovernorId);
                    table.ForeignKey(
                        name: "FK_Governorates_Countrys_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countrys",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CutomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    DateOfRegister = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastLoggedInDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddressCode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CutomerId);
                    table.ForeignKey(
                        name: "FK_Customers_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    TokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GeneratedToken = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    GeneratedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ExpiredDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.TokenId);
                    table.ForeignKey(
                        name: "FK_Tokens_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacies",
                columns: table => new
                {
                    PharmacyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    AddressCode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PhEmail = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    PINCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VedioCallPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DateOfRegister = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.PharmacyId);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Countrys_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countrys",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "GovernorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomersLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateOfLoggedIn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsLogout = table.Column<bool>(type: "bit", nullable: false),
                    DateOfLoggout = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomersLogs_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CutomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefundRequests",
                columns: table => new
                {
                    RefundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateAndTimeOfRequest = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    RefundTypeId = table.Column<int>(type: "int", nullable: false),
                    RefferenceId = table.Column<int>(type: "int", nullable: false),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RefundStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundRequests", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK_RefundRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CutomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefundRequests_RefundStatuses_RefundStatusId",
                        column: x => x.RefundStatusId,
                        principalTable: "RefundStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefundRequests_RefundTypes_RefundTypeId",
                        column: x => x.RefundTypeId,
                        principalTable: "RefundTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    PharamcyId = table.Column<int>(type: "int", nullable: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    LastLoggedInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                    table.ForeignKey(
                        name: "FK_Admins_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admins_Pharmacies_PharamcyId",
                        column: x => x.PharamcyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PharmacyId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    OrderStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CutomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyCurrencies",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PharmacyId = table.Column<int>(type: "int", nullable: false),
                    CurrentCurrency = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyCurrencies", x => x.CurrencyId);
                    table.ForeignKey(
                        name: "FK_PharmacyCurrencies_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyDeliveryLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PharmacyId = table.Column<int>(type: "int", nullable: false),
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyDeliveryLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyDeliveryLocations_DeliveryFees_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "DeliveryFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PharmacyDeliveryLocations_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhPramacyProducts",
                columns: table => new
                {
                    PhProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SysProductId = table.Column<int>(type: "int", nullable: false),
                    PharamcyId = table.Column<int>(type: "int", nullable: false),
                    PhDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Stoke = table.Column<int>(type: "int", nullable: false),
                    ProducedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MedicalTypeId = table.Column<int>(type: "int", nullable: false),
                    MedicalQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhPramacyProducts", x => x.PhProductId);
                    table.ForeignKey(
                        name: "FK_PhPramacyProducts_MedicalTypes_MedicalTypeId",
                        column: x => x.MedicalTypeId,
                        principalTable: "MedicalTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhPramacyProducts_Pharmacies_PharamcyId",
                        column: x => x.PharamcyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhPramacyProducts_SysProducts_SysProductId",
                        column: x => x.SysProductId,
                        principalTable: "SysProducts",
                        principalColumn: "SysProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestMeeting",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    PharmacyId = table.Column<int>(type: "int", nullable: false),
                    RequestDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    RequestStatusId = table.Column<int>(type: "int", nullable: false),
                    MeetingURL = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestMeeting", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_RequestMeeting_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CutomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestMeeting_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestMeeting_RequestStatus_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "RequestStatus",
                        principalColumn: "ReqStatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdminLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    DateOfLoggedIn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsLogout = table.Column<bool>(type: "bit", nullable: false),
                    DateOfLoggout = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminLogs_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    PharmacyId = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DateOfMessage = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "CONVERT(DATE, GETDATE())"),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false, defaultValueSql: "CONVERT(TIME, GETDATE())"),
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CutomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    DateAndTimeOfPaid = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderPayments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateTimeOfPaid = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    PaymentMethodeId = table.Column<int>(type: "int", nullable: false),
                    IsRefund = table.Column<bool>(type: "bit", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPayments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_OrderPayments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CutomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderPayments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderPayments_PaymentMethodes_PaymentMethodeId",
                        column: x => x.PaymentMethodeId,
                        principalTable: "PaymentMethodes",
                        principalColumn: "MethodeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PhProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_PhPramacyProducts_PhProductId",
                        column: x => x.PhProductId,
                        principalTable: "PhPramacyProducts",
                        principalColumn: "PhProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MeetingPayments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateTimeOfPaid = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsRefund = table.Column<bool>(type: "bit", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingPayments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_MeetingPayments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CutomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeetingPayments_RequestMeeting_RequestId",
                        column: x => x.RequestId,
                        principalTable: "RequestMeeting",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    MeetingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JoindAdminId = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.MeetingId);
                    table.ForeignKey(
                        name: "FK_Meetings_Admins_JoindAdminId",
                        column: x => x.JoindAdminId,
                        principalTable: "Admins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Meetings_RequestMeeting_RequestId",
                        column: x => x.RequestId,
                        principalTable: "RequestMeeting",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Countrys",
                columns: new[] { "CountryId", "Name" },
                values: new object[] { 1, "Jordan" });

            migrationBuilder.InsertData(
                table: "DeliveryFees",
                columns: new[] { "Id", "Fees", "MaxDistanceKm", "MinDistanceKm" },
                values: new object[,]
                {
                    { 1, 1m, 3m, 0m },
                    { 2, 2m, 20m, 3.1m },
                    { 3, 3m, 1000m, 20.1m }
                });

            migrationBuilder.InsertData(
                table: "MedicalTypes",
                columns: new[] { "TypeId", "MedicalTypeName", "MedicalTypeNameArabic" },
                values: new object[,]
                {
                    { 1, "Tablet", "أقراص" },
                    { 2, "Capsule", "كبسولات" },
                    { 3, "Syrup", "شراب" },
                    { 4, "Injection", "حقن" },
                    { 5, "Ointment", "مرهم" },
                    { 6, "Cream", "كريم" },
                    { 7, "Gel", "جل" },
                    { 8, "Drop", "قطرات" },
                    { 9, "Inhaler", "بخاخ" },
                    { 10, "Spray", "رذاذ" },
                    { 11, "Cosmetic", "تجميل" },
                    { 12, "Patch", "لاصق" }
                });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Successfull sending" },
                    { 2, "Pending" },
                    { 3, "Preparing" },
                    { 4, "Delivering" },
                    { 5, "Canceled" },
                    { 6, "Not Accepted" },
                    { 7, "Finished" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethodes",
                columns: new[] { "MethodeId", "MethodeName" },
                values: new object[,]
                {
                    { 1, "Paypal" },
                    { 2, "MasterCard" },
                    { 3, "Visa" },
                    { 4, "On Delivery" }
                });

            migrationBuilder.InsertData(
                table: "RefundStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Successfull sending" },
                    { 2, "Pending" },
                    { 3, "Accepted" },
                    { 4, "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "RefundTypes",
                columns: new[] { "TypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Refund Order" },
                    { 2, "Refund Request" }
                });

            migrationBuilder.InsertData(
                table: "RequestStatus",
                columns: new[] { "ReqStatusId", "ReqStatusName" },
                values: new object[,]
                {
                    { 1, "Successfull sending" },
                    { 2, "Pending" },
                    { 3, "Accepted" },
                    { 4, "Not Accepted" },
                    { 5, "Cancled" },
                    { 6, "Finished" }
                });

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "TypeId", "UserTypeName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Customer" }
                });

            migrationBuilder.InsertData(
                table: "Governorates",
                columns: new[] { "GovernorId", "CountryId", "Name", "NameArabic" },
                values: new object[,]
                {
                    { 1, 1, "Amman", "عمان" },
                    { 2, 1, "Zarqa", "الزرقاء" },
                    { 3, 1, "Irbid", "إربد" },
                    { 4, 1, "Ajloun", "عجلون" },
                    { 5, 1, "Jerash", "جرش" },
                    { 6, 1, "Mafraq", "المفرق" },
                    { 7, 1, "Balqa", "البلقاء" },
                    { 8, 1, "Madaba", "مادبا" },
                    { 9, 1, "Karak", "الكرك" },
                    { 10, 1, "Tafilah", "الطفيلة" },
                    { 11, 1, "Ma'an", "معان" },
                    { 12, 1, "Aqaba", "العقبة" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminLogs_AdminId",
                table: "AdminLogs",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_PersonId",
                table: "Admins",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_PharamcyId",
                table: "Admins",
                column: "PharamcyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PersonId",
                table: "Customers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersLogs_CustomerId",
                table: "CustomersLogs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_CountryId",
                table: "Governorates",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OrderId",
                table: "Invoices",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingPayments_CustomerId",
                table: "MeetingPayments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingPayments_RequestId",
                table: "MeetingPayments",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_JoindAdminId",
                table: "Meetings",
                column: "JoindAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_RequestId",
                table: "Meetings",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AdminId",
                table: "Messages",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CustomerId",
                table: "Messages",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_PharmacyId",
                table: "Messages",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserTypeId",
                table: "Messages",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_PhProductId",
                table: "OrderItems",
                column: "PhProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_CustomerId",
                table: "OrderPayments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_OrderId",
                table: "OrderPayments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_PaymentMethodeId",
                table: "OrderPayments",
                column: "PaymentMethodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStatusId",
                table: "Orders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PharmacyId",
                table: "Orders",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_CountryId",
                table: "Pharmacies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_GovernorateId",
                table: "Pharmacies",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyCurrencies_PharmacyId",
                table: "PharmacyCurrencies",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyDeliveryLocations_DeliveryId",
                table: "PharmacyDeliveryLocations",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyDeliveryLocations_PharmacyId",
                table: "PharmacyDeliveryLocations",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_PhPramacyProducts_MedicalTypeId",
                table: "PhPramacyProducts",
                column: "MedicalTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PhPramacyProducts_PharamcyId",
                table: "PhPramacyProducts",
                column: "PharamcyId");

            migrationBuilder.CreateIndex(
                name: "IX_PhPramacyProducts_SysProductId",
                table: "PhPramacyProducts",
                column: "SysProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundRequests_CustomerId",
                table: "RefundRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundRequests_RefundStatusId",
                table: "RefundRequests",
                column: "RefundStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundRequests_RefundTypeId",
                table: "RefundRequests",
                column: "RefundTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMeeting_CustomerId",
                table: "RequestMeeting",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMeeting_PharmacyId",
                table: "RequestMeeting",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMeeting_RequestStatusId",
                table: "RequestMeeting",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserTypeId",
                table: "Tokens",
                column: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminLogs");

            migrationBuilder.DropTable(
                name: "CustomersLogs");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "MeetingPayments");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "OrderPayments");

            migrationBuilder.DropTable(
                name: "PharmacyCurrencies");

            migrationBuilder.DropTable(
                name: "PharmacyDeliveryLocations");

            migrationBuilder.DropTable(
                name: "RefundRequests");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "RequestMeeting");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "PhPramacyProducts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PaymentMethodes");

            migrationBuilder.DropTable(
                name: "DeliveryFees");

            migrationBuilder.DropTable(
                name: "RefundStatuses");

            migrationBuilder.DropTable(
                name: "RefundTypes");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropTable(
                name: "RequestStatus");

            migrationBuilder.DropTable(
                name: "MedicalTypes");

            migrationBuilder.DropTable(
                name: "SysProducts");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DropTable(
                name: "Countrys");
        }
    }
}
