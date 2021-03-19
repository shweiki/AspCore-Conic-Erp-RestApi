using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class Reporttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fktable",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Reports",
                newName: "Style");

            migrationBuilder.RenameColumn(
                name: "TableName",
                table: "Reports",
                newName: "PrintType");

            migrationBuilder.RenameColumn(
                name: "Json",
                table: "Reports",
                newName: "Keys");

            migrationBuilder.RenameColumn(
                name: "HtmlDesgin",
                table: "Reports",
                newName: "Html");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Style",
                table: "Reports",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "PrintType",
                table: "Reports",
                newName: "TableName");

            migrationBuilder.RenameColumn(
                name: "Keys",
                table: "Reports",
                newName: "Json");

            migrationBuilder.RenameColumn(
                name: "Html",
                table: "Reports",
                newName: "HtmlDesgin");

            migrationBuilder.AddColumn<long>(
                name: "Fktable",
                table: "Reports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
