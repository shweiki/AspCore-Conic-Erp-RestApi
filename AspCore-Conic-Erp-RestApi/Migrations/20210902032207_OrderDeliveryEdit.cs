using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class OrderDeliveryEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDelivery_Areas_AreaId",
                table: "OrderDelivery");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDelivery_Drivers_DriverId",
                table: "OrderDelivery");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDelivery_Vendor_VendorId",
                table: "OrderDelivery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDelivery",
                table: "OrderDelivery");

            migrationBuilder.RenameTable(
                name: "OrderDelivery",
                newName: "OrderDeliveries");

            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                table: "OrderDeliveries",
                newName: "TotalPrice");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDelivery_VendorId",
                table: "OrderDeliveries",
                newName: "IX_OrderDeliveries_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDelivery_DriverId",
                table: "OrderDeliveries",
                newName: "IX_OrderDeliveries_DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDelivery_AreaId",
                table: "OrderDeliveries",
                newName: "IX_OrderDeliveries_AreaId");

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "OrderDeliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderDeliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "OrderDeliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDeliveries",
                table: "OrderDeliveries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_Areas_AreaId",
                table: "OrderDeliveries",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_Drivers_DriverId",
                table: "OrderDeliveries",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_Vendor_VendorId",
                table: "OrderDeliveries",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_Areas_AreaId",
                table: "OrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_Drivers_DriverId",
                table: "OrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_Vendor_VendorId",
                table: "OrderDeliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDeliveries",
                table: "OrderDeliveries");

            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "OrderDeliveries");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderDeliveries");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "OrderDeliveries");

            migrationBuilder.RenameTable(
                name: "OrderDeliveries",
                newName: "OrderDelivery");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "OrderDelivery",
                newName: "SellingPrice");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDeliveries_VendorId",
                table: "OrderDelivery",
                newName: "IX_OrderDelivery_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDeliveries_DriverId",
                table: "OrderDelivery",
                newName: "IX_OrderDelivery_DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDeliveries_AreaId",
                table: "OrderDelivery",
                newName: "IX_OrderDelivery_AreaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDelivery",
                table: "OrderDelivery",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDelivery_Areas_AreaId",
                table: "OrderDelivery",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDelivery_Drivers_DriverId",
                table: "OrderDelivery",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDelivery_Vendor_VendorId",
                table: "OrderDelivery",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
