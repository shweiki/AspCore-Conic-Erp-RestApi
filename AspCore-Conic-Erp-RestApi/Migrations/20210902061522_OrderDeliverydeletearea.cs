using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class OrderDeliverydeletearea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_Areas_AreaId",
                table: "OrderDeliveries");

            migrationBuilder.DropIndex(
                name: "IX_OrderDeliveries_AreaId",
                table: "OrderDeliveries");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "OrderDeliveries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AreaId",
                table: "OrderDeliveries",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveries_AreaId",
                table: "OrderDeliveries",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_Areas_AreaId",
                table: "OrderDeliveries",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
