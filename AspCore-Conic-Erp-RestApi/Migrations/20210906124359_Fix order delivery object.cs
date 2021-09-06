using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class Fixorderdeliveryobject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrime",
                table: "OrderDeliveries");

            migrationBuilder.RenameColumn(
                name: "DriverName",
                table: "OrderDeliveries",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "OrderDeliveries",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "OrderDeliveries");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "OrderDeliveries",
                newName: "DriverName");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrime",
                table: "OrderDeliveries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
