using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class AddRoleAdminToDeveloperAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f4c8a1a5-0530-41a3-9ae1-99d51857de42", "4a1971c9-71fd-41d8-8cad-35176671d26a", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f4c8a1a5-0530-41a3-9ae1-99d51857de42", "2c4f9fbb-cefc-4217-909d-dad1b6afd726" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f4c8a1a5-0530-41a3-9ae1-99d51857de42", "2c4f9fbb-cefc-4217-909d-dad1b6afd726" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4c8a1a5-0530-41a3-9ae1-99d51857de42");
        }
    }
}
