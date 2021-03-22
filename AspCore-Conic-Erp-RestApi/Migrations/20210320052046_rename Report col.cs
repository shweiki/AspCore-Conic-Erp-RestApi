using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class renameReportcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Style",
                table: "Reports",
                newName: "Icon");

            migrationBuilder.RenameColumn(
                name: "PrintType",
                table: "Reports",
                newName: "AutoPrint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "Reports",
                newName: "Style");

            migrationBuilder.RenameColumn(
                name: "AutoPrint",
                table: "Reports",
                newName: "PrintType");
        }
    }
}
