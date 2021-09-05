using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class Addvisittableadcont : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrime",
                table: "Visits");

            migrationBuilder.AddColumn<double>(
                name: "HourCount",
                table: "Visits",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourCount",
                table: "Visits");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrime",
                table: "Visits",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
