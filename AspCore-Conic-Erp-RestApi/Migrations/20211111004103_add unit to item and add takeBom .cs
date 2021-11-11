using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class addunittoitemandaddtakeBom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Item",
                newName: "UnitItem");

            migrationBuilder.AddColumn<string>(
                name: "MenuItem",
                table: "Item",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuItem",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "UnitItem",
                table: "Item",
                newName: "Category");
        }
    }
}
