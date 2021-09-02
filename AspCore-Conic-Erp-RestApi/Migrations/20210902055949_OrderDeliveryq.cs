using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class OrderDeliveryq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_Drivers_DriverId",
                table: "OrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_Vendor_VendorId",
                table: "OrderDeliveries");

            migrationBuilder.AlterColumn<long>(
                name: "VendorId",
                table: "OrderDeliveries",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "DriverId",
                table: "OrderDeliveries",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_Drivers_DriverId",
                table: "OrderDeliveries",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_Vendor_VendorId",
                table: "OrderDeliveries",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_Drivers_DriverId",
                table: "OrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_Vendor_VendorId",
                table: "OrderDeliveries");

            migrationBuilder.AlterColumn<long>(
                name: "VendorId",
                table: "OrderDeliveries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DriverId",
                table: "OrderDeliveries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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
    }
}
