using Microsoft.EntityFrameworkCore.Migrations;

namespace RestApi.Migrations
{
    public partial class AddBillOfEnteryIdtoinventorymovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BillOfEnteryId",
                table: "InventoryMovement",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovement_BillOfEnteryId",
                table: "InventoryMovement",
                column: "BillOfEnteryId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_BillOfEnterys_BillOfEnteryId",
                table: "InventoryMovement",
                column: "BillOfEnteryId",
                principalTable: "BillOfEnterys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_BillOfEnterys_BillOfEnteryId",
                table: "InventoryMovement");

            migrationBuilder.DropIndex(
                name: "IX_InventoryMovement_BillOfEnteryId",
                table: "InventoryMovement");

            migrationBuilder.DropColumn(
                name: "BillOfEnteryId",
                table: "InventoryMovement");
        }
    }
}
