using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspCoreConicErpRestApi.Migrations
{
    public partial class AlterActionLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "CashId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "ChequeId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "EntryId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "InventoryMovementId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "ItemsId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "MembershipId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "MembershipMovementId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "MembershipMovementOrderId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "OrderDeliveryId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "OrderInventoryId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "OrderRestaurantId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "PurchaseInvoiceId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "ReceiveId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "SalesInvoiceId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "StockMovementId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "StocktakingInventoryId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "WorkShopId",
                table: "ActionLog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AreaId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "ActionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CashId",
                table: "ActionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChequeId",
                table: "ActionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "ActionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DriverId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EntryId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventoryItemId",
                table: "ActionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InventoryMovementId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ItemsId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MemberId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MembershipId",
                table: "ActionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MembershipMovementId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MembershipMovementOrderId",
                table: "ActionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "ActionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrderDeliveryId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrderInventoryId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrderRestaurantId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "ActionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PaymentId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PurchaseInvoiceId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReceiveId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SalesInvoiceId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "ActionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockMovementId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StocktakingInventoryId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "ActionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VendorId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkShopId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);
        }
    }
}
