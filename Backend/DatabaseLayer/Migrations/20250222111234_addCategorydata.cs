using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class addCategorydata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhPramacyProducts_MedicalCategory_CategoryId",
                table: "PhPramacyProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalCategory",
                table: "MedicalCategory");

            migrationBuilder.RenameTable(
                name: "MedicalCategory",
                newName: "MedicalCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalCategories",
                table: "MedicalCategories",
                column: "CategoryId");

            migrationBuilder.InsertData(
                table: "MedicalCategories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "مسكنات الألم" },
                    { 2, "مضادات حيوية" },
                    { 3, "مضادات الفيروسات" },
                    { 4, "مضادات الفطريات" },
                    { 5, "مضادات الحساسية" },
                    { 6, "مضادات الالتهابات" },
                    { 7, "الفيتامينات والمكملات الغذائية" },
                    { 8, "أدوية الجهاز الهضمي" },
                    { 9, "أدوية الجهاز التنفسي" },
                    { 10, "أدوية القلب والأوعية الدموية" },
                    { 11, "أدوية السكري" },
                    { 12, "أدوية الأعصاب" },
                    { 13, "أدوية الأمراض الجلدية" },
                    { 14, "مستحضرات التجميل والعناية بالبشرة" },
                    { 15, "أدوية العيون" },
                    { 16, "أدوية المسالك البولية" },
                    { 17, "أدوية الهرمونات والغدد الصماء" },
                    { 18, "أدوية الصحة الجنسية" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PhPramacyProducts_MedicalCategories_CategoryId",
                table: "PhPramacyProducts",
                column: "CategoryId",
                principalTable: "MedicalCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhPramacyProducts_MedicalCategories_CategoryId",
                table: "PhPramacyProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalCategories",
                table: "MedicalCategories");

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "MedicalCategories",
                keyColumn: "CategoryId",
                keyValue: 18);

            migrationBuilder.RenameTable(
                name: "MedicalCategories",
                newName: "MedicalCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalCategory",
                table: "MedicalCategory",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhPramacyProducts_MedicalCategory_CategoryId",
                table: "PhPramacyProducts",
                column: "CategoryId",
                principalTable: "MedicalCategory",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
