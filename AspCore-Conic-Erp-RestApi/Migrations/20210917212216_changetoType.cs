using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class changetoType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdjustmentPercentage",
                table: "Adjustments");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Adjustments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Adjustments");

            migrationBuilder.AddColumn<double>(
                name: "AdjustmentPercentage",
                table: "Adjustments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
