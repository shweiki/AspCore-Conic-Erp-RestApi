using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class removesomecolum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoPrint",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AutoSent",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "FooterReport",
                table: "CompanyInfo");

            migrationBuilder.DropColumn(
                name: "HeaderReport",
                table: "CompanyInfo");

            migrationBuilder.AddColumn<float>(
                name: "Total",
                table: "CashPools",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "CashPools");

            migrationBuilder.AddColumn<string>(
                name: "AutoPrint",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AutoSent",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FooterReport",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderReport",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
