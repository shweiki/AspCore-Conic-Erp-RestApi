using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class AreaDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AreaId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DriverId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "ActionLog");
        }
    }
}
