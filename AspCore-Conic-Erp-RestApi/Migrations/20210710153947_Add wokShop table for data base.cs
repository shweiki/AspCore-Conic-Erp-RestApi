using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class AddwokShoptablefordatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "WorkShopId",
                table: "InventoryMovement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkShopId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkShops",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tax = table.Column<double>(type: "float", nullable: true),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmmount = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LowCost = table.Column<double>(type: "float", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkShops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkShops_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovement_WorkShopId",
                table: "InventoryMovement",
                column: "WorkShopId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkShops_VendorId",
                table: "WorkShops",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_WorkShops_WorkShopId",
                table: "InventoryMovement",
                column: "WorkShopId",
                principalTable: "WorkShops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_WorkShops_WorkShopId",
                table: "InventoryMovement");

            migrationBuilder.DropTable(
                name: "WorkShops");

            migrationBuilder.DropIndex(
                name: "IX_InventoryMovement_WorkShopId",
                table: "InventoryMovement");

            migrationBuilder.DropColumn(
                name: "WorkShopId",
                table: "InventoryMovement");

            migrationBuilder.DropColumn(
                name: "WorkShopId",
                table: "ActionLog");
        }
    }
}
