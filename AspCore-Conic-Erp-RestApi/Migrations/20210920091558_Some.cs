using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class Some : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdjustmentPercentage",
                table: "StaticAdjustments");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StaticAdjustments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "StaticAdjustments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "StaticAdjustments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "StaticAdjustments");

            migrationBuilder.AddColumn<double>(
                name: "AdjustmentPercentage",
                table: "StaticAdjustments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
