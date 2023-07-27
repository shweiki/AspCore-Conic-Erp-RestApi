using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class SortTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionLog_Oprationsys",
                table: "ActionLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Bank_Account",
                table: "Bank");

            migrationBuilder.DropForeignKey(
                name: "FK_Cheque_Vendor",
                table: "Cheque");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceLog_Device",
                table: "DeviceLog");

            migrationBuilder.DropForeignKey(
                name: "FK_EntryMovement_Account",
                table: "EntryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_EntryMovement_EntryAccounting",
                table: "EntryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_BillOfEnterys_BillOfEnteryId",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_InventoryItem",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_Item",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_OrderInventory",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_PurchaseInvoice",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_SalesInvoice",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_WorkShops_WorkShopId",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMUO_Item",
                table: "ItemMUO");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMUO_MenuItem",
                table: "ItemMUO");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMUO_OriginItem",
                table: "ItemMUO");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMUO_UnitItem",
                table: "ItemMUO");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Account",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipMovement_Member",
                table: "MembershipMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipMovement_Membership",
                table: "MembershipMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipMovementOrder_MembershipMovement",
                table: "MembershipMovementOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Member",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Vendor",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseInvoice_Vendor",
                table: "PurchaseInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoice_Member",
                table: "SalesInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoice_Vendor",
                table: "SalesInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Item",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_InventoryItem",
                table: "StockMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_Item",
                table: "StockMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_StocktakingInventory",
                table: "StockMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_Areas_AreaId",
                table: "Vendor");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "BillOfEnterys");

            migrationBuilder.DropTable(
                name: "CashPools");

            migrationBuilder.DropTable(
                name: "Oprationsys");

            migrationBuilder.DropTable(
                name: "OrderDeliveries");

            migrationBuilder.DropTable(
                name: "OrderRestaurants");

            migrationBuilder.DropTable(
                name: "Receives");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "SalaryAdjustmentLogs");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "WorkShops");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Adjustment");

            migrationBuilder.DropTable(
                name: "SalaryPayments");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemMUO",
                table: "ItemMUO");

            migrationBuilder.DropIndex(
                name: "IX_DeviceLog_Status",
                table: "DeviceLog");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2890efde-5d28-406d-ae5e-72576f74870f", "2c4f9fbb-cefc-4217-909d-dad1b6afd726" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f4c8a1a5-0530-41a3-9ae1-99d51857de42", "2c4f9fbb-cefc-4217-909d-dad1b6afd726" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2890efde-5d28-406d-ae5e-72576f74870f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4c8a1a5-0530-41a3-9ae1-99d51857de42");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c4f9fbb-cefc-4217-909d-dad1b6afd726");

            migrationBuilder.RenameTable(
                name: "ItemMUO",
                newName: "ItemMuo");

            migrationBuilder.RenameIndex(
                name: "IX_StockMovement_StocktakingInventoryID",
                table: "StockMovement",
                newName: "IX_StockMovement_StocktakingInventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_StockMovement_ItemsID",
                table: "StockMovement",
                newName: "IX_StockMovement_ItemsId");

            migrationBuilder.RenameIndex(
                name: "IX_StockMovement_InventoryItemID",
                table: "StockMovement",
                newName: "IX_StockMovement_InventoryItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Service_ItemID",
                table: "Service",
                newName: "IX_Service_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesInvoice_VendorID",
                table: "SalesInvoice",
                newName: "IX_SalesInvoice_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesInvoice_MemberID",
                table: "SalesInvoice",
                newName: "IX_SalesInvoice_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseInvoice_VendorID",
                table: "PurchaseInvoice",
                newName: "IX_PurchaseInvoice_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_VendorID",
                table: "Payment",
                newName: "IX_Payment_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_MemberID",
                table: "Payment",
                newName: "IX_Payment_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipMovementOrder_MemberShipMovementID",
                table: "MembershipMovementOrder",
                newName: "IX_MembershipMovementOrder_MemberShipMovementId");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipMovement_MembershipID",
                table: "MembershipMovement",
                newName: "IX_MembershipMovement_MembershipId");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipMovement_MemberID",
                table: "MembershipMovement",
                newName: "IX_MembershipMovement_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Member_AccountID",
                table: "Member",
                newName: "IX_Member_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMUO_UnitItemID",
                table: "ItemMuo",
                newName: "IX_ItemMuo_UnitItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMUO_OriginItemID",
                table: "ItemMuo",
                newName: "IX_ItemMuo_OriginItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMUO_MenuItemID",
                table: "ItemMuo",
                newName: "IX_ItemMuo_MenuItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMUO_ItemsID",
                table: "ItemMuo",
                newName: "IX_ItemMuo_ItemsId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovement_SalesInvoiceID",
                table: "InventoryMovement",
                newName: "IX_InventoryMovement_SalesInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovement_PurchaseInvoiceID",
                table: "InventoryMovement",
                newName: "IX_InventoryMovement_PurchaseInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovement_OrderInventoryID",
                table: "InventoryMovement",
                newName: "IX_InventoryMovement_OrderInventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovement_ItemsID",
                table: "InventoryMovement",
                newName: "IX_InventoryMovement_ItemsId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovement_InventoryItemID",
                table: "InventoryMovement",
                newName: "IX_InventoryMovement_InventoryItemId");

            migrationBuilder.RenameIndex(
                name: "IX_EntryMovement_EntryID",
                table: "EntryMovement",
                newName: "IX_EntryMovement_EntryId");

            migrationBuilder.RenameIndex(
                name: "IX_EntryMovement_AccountID",
                table: "EntryMovement",
                newName: "IX_EntryMovement_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceLog_DeviceID",
                table: "DeviceLog",
                newName: "IX_DeviceLog_DeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_Cheque_VendorID",
                table: "Cheque",
                newName: "IX_Cheque_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Bank_AccountID",
                table: "Bank",
                newName: "IX_Bank_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_ActionLog_OprationID",
                table: "ActionLog",
                newName: "IX_ActionLog_OprationId");

            migrationBuilder.AlterDatabase(
                oldCollation: "Arabic_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnitItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StockMovement",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "SalesInvoice",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "PurchaseInvoice",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "AccountInvoiceNumber",
                table: "PurchaseInvoice",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OriginItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "OrderType",
                table: "OrderInventory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MenuItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MembershipMovementOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MembershipMovement",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DiscountDescription",
                table: "MembershipMovement",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Membership",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Membership",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ssn",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber2",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber1",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Massage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "Massage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Massage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Massage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ItemMuo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Item",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Item",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Item",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TypeMove",
                table: "InventoryMovement",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "InventoryMovement",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Str",
                table: "FingerPrint",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "FileData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                table: "FileData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "File",
                table: "FileData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "EntryAccounting",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EditorsUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Discount",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Discount",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Discount",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "DeviceLog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Device",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "Device",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Device",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RateNumber",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber2",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber1",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Fax",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentType",
                table: "Cheque",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cheque",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Cheque",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(100)",
                oldFixedLength: true,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankAddress",
                table: "Cheque",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Bank",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Bank",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Bank",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(100)",
                oldFixedLength: true,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BranchName",
                table: "Bank",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountType",
                table: "Bank",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(100)",
                oldFixedLength: true,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BackUp",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "DataBaseName",
                table: "BackUp",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BackUpPath",
                table: "BackUp",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadSignConsent",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ActionLog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemMuo",
                table: "ItemMuo",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Adjustment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdjustmentAmount = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsStaticAdjustment = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adjustment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DelievryPrice = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BillOfEntery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemsIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ST9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PurchaseInvoiceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillOfEntery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashPool",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<float>(type: "real", nullable: false),
                    TotalCash = table.Column<float>(type: "real", nullable: false),
                    TotalCoins = table.Column<float>(type: "real", nullable: false),
                    TotalVisa = table.Column<float>(type: "real", nullable: false),
                    TotalReject = table.Column<float>(type: "real", nullable: false),
                    TotalOutlay = table.Column<float>(type: "real", nullable: false),
                    TotalRestitution = table.Column<float>(type: "real", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fktable = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashPool", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Driver",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ssn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LatinName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Ssn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vaccine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Oprationsy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OprationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReferenceStatus = table.Column<int>(type: "int", nullable: true),
                    OprationDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicOprationDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oprationsy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderRestaurant",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPill = table.Column<double>(type: "float", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TableNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRestaurant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderRestaurant_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Receive",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceiveMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmmount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: true),
                    IsPrime = table.Column<bool>(type: "bit", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditorName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receive_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Receive_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailSent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Printer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Html = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Visit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tax = table.Column<double>(type: "float", nullable: true),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PersonCount = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HourCount = table.Column<double>(type: "float", nullable: false),
                    HourPrice = table.Column<double>(type: "float", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkShop",
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
                    table.PrimaryKey("PK_WorkShop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkShop_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDelivery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPill = table.Column<double>(type: "float", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryPrice = table.Column<double>(type: "float", nullable: false),
                    DriverId = table.Column<long>(type: "bigint", nullable: true),
                    VendorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDelivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDelivery_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDelivery_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalaryPayment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    GrossSalary = table.Column<double>(type: "float", nullable: false),
                    NetSalary = table.Column<double>(type: "float", nullable: false),
                    SalaryFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalaryTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    WorkingHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryPayment_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalaryAdjustmentLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdjustmentAmmount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdjustmentId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryPaymentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryAdjustmentLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryAdjustmentLog_Adjustment_AdjustmentId",
                        column: x => x.AdjustmentId,
                        principalTable: "Adjustment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryAdjustmentLog_SalaryPayment_SalaryPaymentId",
                        column: x => x.SalaryPaymentId,
                        principalTable: "SalaryPayment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_AccountId",
                table: "Employee",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDelivery_DriverId",
                table: "OrderDelivery",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDelivery_VendorId",
                table: "OrderDelivery",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRestaurant_VendorId",
                table: "OrderRestaurant",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Receive_MemberId",
                table: "Receive",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Receive_VendorId",
                table: "Receive",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdjustmentLog_AdjustmentId",
                table: "SalaryAdjustmentLog",
                column: "AdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdjustmentLog_SalaryPaymentId",
                table: "SalaryAdjustmentLog",
                column: "SalaryPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryPayment_EmployeeId",
                table: "SalaryPayment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkShop_VendorId",
                table: "WorkShop",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionLog_Oprationsy_OprationId",
                table: "ActionLog",
                column: "OprationId",
                principalTable: "Oprationsy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bank_Account_AccountId",
                table: "Bank",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheque_Vendor_VendorId",
                table: "Cheque",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceLog_Device_DeviceId",
                table: "DeviceLog",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryMovement_Account_AccountId",
                table: "EntryMovement",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryMovement_EntryAccounting_EntryId",
                table: "EntryMovement",
                column: "EntryId",
                principalTable: "EntryAccounting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_BillOfEntery_BillOfEnteryId",
                table: "InventoryMovement",
                column: "BillOfEnteryId",
                principalTable: "BillOfEntery",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_InventoryItem_InventoryItemId",
                table: "InventoryMovement",
                column: "InventoryItemId",
                principalTable: "InventoryItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_Item_ItemsId",
                table: "InventoryMovement",
                column: "ItemsId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_OrderInventory_OrderInventoryId",
                table: "InventoryMovement",
                column: "OrderInventoryId",
                principalTable: "OrderInventory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_PurchaseInvoice_PurchaseInvoiceId",
                table: "InventoryMovement",
                column: "PurchaseInvoiceId",
                principalTable: "PurchaseInvoice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_SalesInvoice_SalesInvoiceId",
                table: "InventoryMovement",
                column: "SalesInvoiceId",
                principalTable: "SalesInvoice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_WorkShop_WorkShopId",
                table: "InventoryMovement",
                column: "WorkShopId",
                principalTable: "WorkShop",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMuo_Item_ItemsId",
                table: "ItemMuo",
                column: "ItemsId",
                principalTable: "Item",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMuo_MenuItem_MenuItemId",
                table: "ItemMuo",
                column: "MenuItemId",
                principalTable: "MenuItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMuo_OriginItem_OriginItemId",
                table: "ItemMuo",
                column: "OriginItemId",
                principalTable: "OriginItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMuo_UnitItem_UnitItemId",
                table: "ItemMuo",
                column: "UnitItemId",
                principalTable: "UnitItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Account_AccountId",
                table: "Member",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipMovement_Member_MemberId",
                table: "MembershipMovement",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipMovement_Membership_MembershipId",
                table: "MembershipMovement",
                column: "MembershipId",
                principalTable: "Membership",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipMovementOrder_MembershipMovement_MemberShipMovementId",
                table: "MembershipMovementOrder",
                column: "MemberShipMovementId",
                principalTable: "MembershipMovement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Member_MemberId",
                table: "Payment",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Vendor_VendorId",
                table: "Payment",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoice_Vendor_VendorId",
                table: "PurchaseInvoice",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoice_Member_MemberId",
                table: "SalesInvoice",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoice_Vendor_VendorId",
                table: "SalesInvoice",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Item_ItemId",
                table: "Service",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovement_InventoryItem_InventoryItemId",
                table: "StockMovement",
                column: "InventoryItemId",
                principalTable: "InventoryItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovement_Item_ItemsId",
                table: "StockMovement",
                column: "ItemsId",
                principalTable: "Item",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovement_StocktakingInventory_StocktakingInventoryId",
                table: "StockMovement",
                column: "StocktakingInventoryId",
                principalTable: "StocktakingInventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_Area_AreaId",
                table: "Vendor",
                column: "AreaId",
                principalTable: "Area",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionLog_Oprationsy_OprationId",
                table: "ActionLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Bank_Account_AccountId",
                table: "Bank");

            migrationBuilder.DropForeignKey(
                name: "FK_Cheque_Vendor_VendorId",
                table: "Cheque");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceLog_Device_DeviceId",
                table: "DeviceLog");

            migrationBuilder.DropForeignKey(
                name: "FK_EntryMovement_Account_AccountId",
                table: "EntryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_EntryMovement_EntryAccounting_EntryId",
                table: "EntryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_BillOfEntery_BillOfEnteryId",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_InventoryItem_InventoryItemId",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_Item_ItemsId",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_OrderInventory_OrderInventoryId",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_PurchaseInvoice_PurchaseInvoiceId",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_SalesInvoice_SalesInvoiceId",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_WorkShop_WorkShopId",
                table: "InventoryMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMuo_Item_ItemsId",
                table: "ItemMuo");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMuo_MenuItem_MenuItemId",
                table: "ItemMuo");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMuo_OriginItem_OriginItemId",
                table: "ItemMuo");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMuo_UnitItem_UnitItemId",
                table: "ItemMuo");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Account_AccountId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipMovement_Member_MemberId",
                table: "MembershipMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipMovement_Membership_MembershipId",
                table: "MembershipMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipMovementOrder_MembershipMovement_MemberShipMovementId",
                table: "MembershipMovementOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Member_MemberId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Vendor_VendorId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseInvoice_Vendor_VendorId",
                table: "PurchaseInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoice_Member_MemberId",
                table: "SalesInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoice_Vendor_VendorId",
                table: "SalesInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Item_ItemId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_InventoryItem_InventoryItemId",
                table: "StockMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_Item_ItemsId",
                table: "StockMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_StocktakingInventory_StocktakingInventoryId",
                table: "StockMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_Area_AreaId",
                table: "Vendor");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "BillOfEntery");

            migrationBuilder.DropTable(
                name: "CashPool");

            migrationBuilder.DropTable(
                name: "Oprationsy");

            migrationBuilder.DropTable(
                name: "OrderDelivery");

            migrationBuilder.DropTable(
                name: "OrderRestaurant");

            migrationBuilder.DropTable(
                name: "Receive");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "SalaryAdjustmentLog");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "Visit");

            migrationBuilder.DropTable(
                name: "WorkShop");

            migrationBuilder.DropTable(
                name: "Driver");

            migrationBuilder.DropTable(
                name: "Adjustment");

            migrationBuilder.DropTable(
                name: "SalaryPayment");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemMuo",
                table: "ItemMuo");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsReadSignConsent",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "ItemMuo",
                newName: "ItemMUO");

            migrationBuilder.RenameIndex(
                name: "IX_StockMovement_StocktakingInventoryId",
                table: "StockMovement",
                newName: "IX_StockMovement_StocktakingInventoryID");

            migrationBuilder.RenameIndex(
                name: "IX_StockMovement_ItemsId",
                table: "StockMovement",
                newName: "IX_StockMovement_ItemsID");

            migrationBuilder.RenameIndex(
                name: "IX_StockMovement_InventoryItemId",
                table: "StockMovement",
                newName: "IX_StockMovement_InventoryItemID");

            migrationBuilder.RenameIndex(
                name: "IX_Service_ItemId",
                table: "Service",
                newName: "IX_Service_ItemID");

            migrationBuilder.RenameIndex(
                name: "IX_SalesInvoice_VendorId",
                table: "SalesInvoice",
                newName: "IX_SalesInvoice_VendorID");

            migrationBuilder.RenameIndex(
                name: "IX_SalesInvoice_MemberId",
                table: "SalesInvoice",
                newName: "IX_SalesInvoice_MemberID");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseInvoice_VendorId",
                table: "PurchaseInvoice",
                newName: "IX_PurchaseInvoice_VendorID");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_VendorId",
                table: "Payment",
                newName: "IX_Payment_VendorID");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_MemberId",
                table: "Payment",
                newName: "IX_Payment_MemberID");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipMovementOrder_MemberShipMovementId",
                table: "MembershipMovementOrder",
                newName: "IX_MembershipMovementOrder_MemberShipMovementID");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipMovement_MembershipId",
                table: "MembershipMovement",
                newName: "IX_MembershipMovement_MembershipID");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipMovement_MemberId",
                table: "MembershipMovement",
                newName: "IX_MembershipMovement_MemberID");

            migrationBuilder.RenameIndex(
                name: "IX_Member_AccountId",
                table: "Member",
                newName: "IX_Member_AccountID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMuo_UnitItemId",
                table: "ItemMUO",
                newName: "IX_ItemMUO_UnitItemID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMuo_OriginItemId",
                table: "ItemMUO",
                newName: "IX_ItemMUO_OriginItemID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMuo_MenuItemId",
                table: "ItemMUO",
                newName: "IX_ItemMUO_MenuItemID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMuo_ItemsId",
                table: "ItemMUO",
                newName: "IX_ItemMUO_ItemsID");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovement_SalesInvoiceId",
                table: "InventoryMovement",
                newName: "IX_InventoryMovement_SalesInvoiceID");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovement_PurchaseInvoiceId",
                table: "InventoryMovement",
                newName: "IX_InventoryMovement_PurchaseInvoiceID");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovement_OrderInventoryId",
                table: "InventoryMovement",
                newName: "IX_InventoryMovement_OrderInventoryID");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovement_ItemsId",
                table: "InventoryMovement",
                newName: "IX_InventoryMovement_ItemsID");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovement_InventoryItemId",
                table: "InventoryMovement",
                newName: "IX_InventoryMovement_InventoryItemID");

            migrationBuilder.RenameIndex(
                name: "IX_EntryMovement_EntryId",
                table: "EntryMovement",
                newName: "IX_EntryMovement_EntryID");

            migrationBuilder.RenameIndex(
                name: "IX_EntryMovement_AccountId",
                table: "EntryMovement",
                newName: "IX_EntryMovement_AccountID");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceLog_DeviceId",
                table: "DeviceLog",
                newName: "IX_DeviceLog_DeviceID");

            migrationBuilder.RenameIndex(
                name: "IX_Cheque_VendorId",
                table: "Cheque",
                newName: "IX_Cheque_VendorID");

            migrationBuilder.RenameIndex(
                name: "IX_Bank_AccountId",
                table: "Bank",
                newName: "IX_Bank_AccountID");

            migrationBuilder.RenameIndex(
                name: "IX_ActionLog_OprationId",
                table: "ActionLog",
                newName: "IX_ActionLog_OprationID");

            migrationBuilder.AlterDatabase(
                collation: "Arabic_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnitItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StockMovement",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Service",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Service",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Service",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "SalesInvoice",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "PurchaseInvoice",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountInvoiceNumber",
                table: "PurchaseInvoice",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payment",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OriginItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrderType",
                table: "OrderInventory",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MenuItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MembershipMovementOrder",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MembershipMovement",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiscountDescription",
                table: "MembershipMovement",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Membership",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Membership",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Member",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "Member",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ssn",
                table: "Member",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber2",
                table: "Member",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber1",
                table: "Member",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Member",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Member",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Massage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "Massage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Massage",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Massage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ItemMUO",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Item",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Item",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Item",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TypeMove",
                table: "InventoryMovement",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "InventoryMovement",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Str",
                table: "FingerPrint",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "FileData",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                table: "FileData",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "File",
                table: "FileData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "EntryAccounting",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EditorsUser",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Discount",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Discount",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Discount",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "DeviceLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Device",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "Device",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Device",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "CompanyInfo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "CompanyInfo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RateNumber",
                table: "CompanyInfo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber2",
                table: "CompanyInfo",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber1",
                table: "CompanyInfo",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                table: "CompanyInfo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CompanyInfo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Fax",
                table: "CompanyInfo",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "CompanyInfo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CompanyInfo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentType",
                table: "Cheque",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cheque",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Cheque",
                type: "nchar(100)",
                fixedLength: true,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankAddress",
                table: "Cheque",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Bank",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Bank",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Bank",
                type: "nchar(100)",
                fixedLength: true,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BranchName",
                table: "Bank",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountType",
                table: "Bank",
                type: "nchar(100)",
                fixedLength: true,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BackUp",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataBaseName",
                table: "BackUp",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BackUpPath",
                table: "BackUp",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ActionLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemMUO",
                table: "ItemMUO",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Adjustment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdjustmentAmount = table.Column<double>(type: "float", nullable: false),
                    IsStaticAdjustment = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adjustment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DelievryPrice = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BillOfEnterys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItemsIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseInvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    ST9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillOfEnterys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashPools",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fktable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<float>(type: "real", nullable: false),
                    TotalCash = table.Column<float>(type: "real", nullable: false),
                    TotalCoins = table.Column<float>(type: "real", nullable: false),
                    TotalOutlay = table.Column<float>(type: "real", nullable: false),
                    TotalReject = table.Column<float>(type: "real", nullable: false),
                    TotalRestitution = table.Column<float>(type: "real", nullable: false),
                    TotalVisa = table.Column<float>(type: "real", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashPools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ssn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LatinName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ssn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vaccine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Oprationsys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArabicOprationDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OprationDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OprationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ReferenceStatus = table.Column<int>(type: "int", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oprationsys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderRestaurants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<long>(type: "bigint", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TableNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPill = table.Column<double>(type: "float", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRestaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderRestaurants_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Receives",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<long>(type: "bigint", nullable: true),
                    VendorId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPrime = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiveMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmmount = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receives_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Receives_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailSent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Html = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Printer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HourCount = table.Column<double>(type: "float", nullable: false),
                    HourPrice = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonCount = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<double>(type: "float", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkShops",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LowCost = table.Column<double>(type: "float", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<double>(type: "float", nullable: true),
                    TotalAmmount = table.Column<double>(type: "float", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "OrderDeliveries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<long>(type: "bigint", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryPrice = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalPill = table.Column<double>(type: "float", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDeliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDeliveries_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDeliveries_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalaryPayments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    GrossSalary = table.Column<double>(type: "float", nullable: false),
                    NetSalary = table.Column<double>(type: "float", nullable: false),
                    SalaryFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalaryTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    WorkingHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryPayments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalaryAdjustmentLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdjustmentId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryPaymentId = table.Column<long>(type: "bigint", nullable: true),
                    AdjustmentAmmount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryAdjustmentLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryAdjustmentLogs_Adjustment_AdjustmentId",
                        column: x => x.AdjustmentId,
                        principalTable: "Adjustment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryAdjustmentLogs_SalaryPayments_SalaryPaymentId",
                        column: x => x.SalaryPaymentId,
                        principalTable: "SalaryPayments",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2890efde-5d28-406d-ae5e-72576f74870f", "f9fd5fb8-1ea9-4afd-8029-b8a027ee943f", "Developer", "DEVELOPER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f4c8a1a5-0530-41a3-9ae1-99d51857de42", "4a1971c9-71fd-41d8-8cad-35176671d26a", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2c4f9fbb-cefc-4217-909d-dad1b6afd726", 0, "b41b9a0f-f0df-4826-a8cf-0733c0c94f56", "tahashweiki.1994@Gmail.com", true, true, null, "TAHASHWEIKI.1994@GMAIL.COM", "DEVELOPER", "AQAAAAEAACcQAAAAEDlKsBqScI1exq/bzxvkvaDbqjeVK5MbABg6aA9S8KbO9QRBnSO79l9grdjvH9+gMg==", "00962788675843", true, "IZEQASXPA5Z6U7O2RPM32FSODDDDDIOW", false, "Developer" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2890efde-5d28-406d-ae5e-72576f74870f", "2c4f9fbb-cefc-4217-909d-dad1b6afd726" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f4c8a1a5-0530-41a3-9ae1-99d51857de42", "2c4f9fbb-cefc-4217-909d-dad1b6afd726" });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLog_Status",
                table: "DeviceLog",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AccountId",
                table: "Employees",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveries_DriverId",
                table: "OrderDeliveries",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveries_VendorId",
                table: "OrderDeliveries",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRestaurants_VendorId",
                table: "OrderRestaurants",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_MemberId",
                table: "Receives",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_VendorId",
                table: "Receives",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdjustmentLogs_AdjustmentId",
                table: "SalaryAdjustmentLogs",
                column: "AdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdjustmentLogs_SalaryPaymentId",
                table: "SalaryAdjustmentLogs",
                column: "SalaryPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryPayments_EmployeeId",
                table: "SalaryPayments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkShops_VendorId",
                table: "WorkShops",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionLog_Oprationsys",
                table: "ActionLog",
                column: "OprationId",
                principalTable: "Oprationsys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bank_Account",
                table: "Bank",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheque_Vendor",
                table: "Cheque",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceLog_Device",
                table: "DeviceLog",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EntryMovement_Account",
                table: "EntryMovement",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EntryMovement_EntryAccounting",
                table: "EntryMovement",
                column: "EntryId",
                principalTable: "EntryAccounting",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_BillOfEnterys_BillOfEnteryId",
                table: "InventoryMovement",
                column: "BillOfEnteryId",
                principalTable: "BillOfEnterys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_InventoryItem",
                table: "InventoryMovement",
                column: "InventoryItemId",
                principalTable: "InventoryItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_Item",
                table: "InventoryMovement",
                column: "ItemsId",
                principalTable: "Item",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_OrderInventory",
                table: "InventoryMovement",
                column: "OrderInventoryId",
                principalTable: "OrderInventory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_PurchaseInvoice",
                table: "InventoryMovement",
                column: "PurchaseInvoiceId",
                principalTable: "PurchaseInvoice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_SalesInvoice",
                table: "InventoryMovement",
                column: "SalesInvoiceId",
                principalTable: "SalesInvoice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_WorkShops_WorkShopId",
                table: "InventoryMovement",
                column: "WorkShopId",
                principalTable: "WorkShops",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMUO_Item",
                table: "ItemMUO",
                column: "ItemsId",
                principalTable: "Item",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMUO_MenuItem",
                table: "ItemMUO",
                column: "MenuItemId",
                principalTable: "MenuItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMUO_OriginItem",
                table: "ItemMUO",
                column: "OriginItemId",
                principalTable: "OriginItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMUO_UnitItem",
                table: "ItemMUO",
                column: "UnitItemId",
                principalTable: "UnitItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Account",
                table: "Member",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipMovement_Member",
                table: "MembershipMovement",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipMovement_Membership",
                table: "MembershipMovement",
                column: "MembershipId",
                principalTable: "Membership",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipMovementOrder_MembershipMovement",
                table: "MembershipMovementOrder",
                column: "MemberShipMovementId",
                principalTable: "MembershipMovement",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Member",
                table: "Payment",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Vendor",
                table: "Payment",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoice_Vendor",
                table: "PurchaseInvoice",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoice_Member",
                table: "SalesInvoice",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoice_Vendor",
                table: "SalesInvoice",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Item",
                table: "Service",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovement_InventoryItem",
                table: "StockMovement",
                column: "InventoryItemId",
                principalTable: "InventoryItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovement_Item",
                table: "StockMovement",
                column: "ItemsId",
                principalTable: "Item",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovement_StocktakingInventory",
                table: "StockMovement",
                column: "StocktakingInventoryId",
                principalTable: "StocktakingInventory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_Areas_AreaId",
                table: "Vendor",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");
        }
    }
}
