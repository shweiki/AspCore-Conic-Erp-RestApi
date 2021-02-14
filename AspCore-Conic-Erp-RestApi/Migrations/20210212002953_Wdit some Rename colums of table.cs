using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class WditsomeRenamecolumsoftable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "Vendor",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Vendor",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "UnitItem",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "StocktakingInventory",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StocktakingInventoryID",
                table: "StockMovement",
                newName: "StocktakingInventoryId");

            migrationBuilder.RenameColumn(
                name: "ItemsID",
                table: "StockMovement",
                newName: "ItemsId");

            migrationBuilder.RenameColumn(
                name: "InventoryItemID",
                table: "StockMovement",
                newName: "InventoryItemId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "StockMovement",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ItemID",
                table: "Service",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Service",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "VendorID",
                table: "SalesInvoice",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "MemberID",
                table: "SalesInvoice",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SalesInvoice",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "VendorID",
                table: "PurchaseInvoice",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PurchaseInvoice",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "VendorID",
                table: "Payment",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "MemberID",
                table: "Payment",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Payment",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "OriginItem",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "OrderInventory",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Oprationsys",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MenuItem",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MemberShipMovementID",
                table: "MembershipMovementOrder",
                newName: "MemberShipMovementId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MembershipMovementOrder",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MembershipID",
                table: "MembershipMovement",
                newName: "MembershipId");

            migrationBuilder.RenameColumn(
                name: "MemberID",
                table: "MembershipMovement",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MembershipMovement",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Membership",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MemberID",
                table: "MemberLog",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "DeviceID",
                table: "MemberLog",
                newName: "DeviceId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MemberLog",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MemberID",
                table: "MemberFace",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MemberFace",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SSN",
                table: "Member",
                newName: "Ssn");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "Member",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Member",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FKTable",
                table: "Massage",
                newName: "Fktable");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Massage",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UnitItemID",
                table: "ItemMUO",
                newName: "UnitItemId");

            migrationBuilder.RenameColumn(
                name: "OriginItemID",
                table: "ItemMUO",
                newName: "OriginItemId");

            migrationBuilder.RenameColumn(
                name: "MenuItemID",
                table: "ItemMUO",
                newName: "MenuItemId");

            migrationBuilder.RenameColumn(
                name: "ItemsID",
                table: "ItemMUO",
                newName: "ItemsId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ItemMUO",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Item",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SalesInvoiceID",
                table: "InventoryMovement",
                newName: "SalesInvoiceId");

            migrationBuilder.RenameColumn(
                name: "PurchaseInvoiceID",
                table: "InventoryMovement",
                newName: "PurchaseInvoiceId");

            migrationBuilder.RenameColumn(
                name: "OrderInventoryID",
                table: "InventoryMovement",
                newName: "OrderInventoryId");

            migrationBuilder.RenameColumn(
                name: "ItemsID",
                table: "InventoryMovement",
                newName: "ItemsId");

            migrationBuilder.RenameColumn(
                name: "InventoryItemID",
                table: "InventoryMovement",
                newName: "InventoryItemId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "InventoryMovement",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "InventoryItem",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FKTable",
                table: "FileData",
                newName: "Fktable");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "FileData",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EntryID",
                table: "EntryMovement",
                newName: "EntryId");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "EntryMovement",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "EntryMovement",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "EntryAccounting",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "EditorsUser",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Discount",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IP",
                table: "Device",
                newName: "Ip");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Device",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CompanyInfo",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "VendorID",
                table: "Cheque",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Cheque",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PCIP",
                table: "Cash",
                newName: "Pcip");

            migrationBuilder.RenameColumn(
                name: "BTCash",
                table: "Cash",
                newName: "Btcash");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "Cash",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Cash",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IBAN",
                table: "Bank",
                newName: "Iban");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "Bank",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Bank",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "BackUp",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "BackUp",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "VendorID",
                table: "ActionLog",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "ActionLog",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UnitID",
                table: "ActionLog",
                newName: "UnitId");

            migrationBuilder.RenameColumn(
                name: "StocktakingInventoryID",
                table: "ActionLog",
                newName: "StocktakingInventoryId");

            migrationBuilder.RenameColumn(
                name: "StockMovementID",
                table: "ActionLog",
                newName: "StockMovementId");

            migrationBuilder.RenameColumn(
                name: "ServiceID",
                table: "ActionLog",
                newName: "ServiceId");

            migrationBuilder.RenameColumn(
                name: "SalesInvoiceID",
                table: "ActionLog",
                newName: "SalesInvoiceId");

            migrationBuilder.RenameColumn(
                name: "PurchaseInvoiceID",
                table: "ActionLog",
                newName: "PurchaseInvoiceId");

            migrationBuilder.RenameColumn(
                name: "PaymentID",
                table: "ActionLog",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "OriginID",
                table: "ActionLog",
                newName: "OriginId");

            migrationBuilder.RenameColumn(
                name: "OrderInventoryID",
                table: "ActionLog",
                newName: "OrderInventoryId");

            migrationBuilder.RenameColumn(
                name: "OprationID",
                table: "ActionLog",
                newName: "OprationId");

            migrationBuilder.RenameColumn(
                name: "MenuID",
                table: "ActionLog",
                newName: "MenuId");

            migrationBuilder.RenameColumn(
                name: "MembershipMovementOrderID",
                table: "ActionLog",
                newName: "MembershipMovementOrderId");

            migrationBuilder.RenameColumn(
                name: "MembershipMovementID",
                table: "ActionLog",
                newName: "MembershipMovementId");

            migrationBuilder.RenameColumn(
                name: "MembershipID",
                table: "ActionLog",
                newName: "MembershipId");

            migrationBuilder.RenameColumn(
                name: "MemberID",
                table: "ActionLog",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "ItemsID",
                table: "ActionLog",
                newName: "ItemsId");

            migrationBuilder.RenameColumn(
                name: "InventoryMovementID",
                table: "ActionLog",
                newName: "InventoryMovementId");

            migrationBuilder.RenameColumn(
                name: "InventoryItemID",
                table: "ActionLog",
                newName: "InventoryItemId");

            migrationBuilder.RenameColumn(
                name: "EntryID",
                table: "ActionLog",
                newName: "EntryId");

            migrationBuilder.RenameColumn(
                name: "DiscountID",
                table: "ActionLog",
                newName: "DiscountId");

            migrationBuilder.RenameColumn(
                name: "ChequeID",
                table: "ActionLog",
                newName: "ChequeId");

            migrationBuilder.RenameColumn(
                name: "CashID",
                table: "ActionLog",
                newName: "CashId");

            migrationBuilder.RenameColumn(
                name: "BankID",
                table: "ActionLog",
                newName: "BankId");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "ActionLog",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ActionLog",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Account",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnitItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "StocktakingInventory",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StockMovement",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Service",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Service",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Service",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "SalesInvoice",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SalesInvoice",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "PurchaseInvoice",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PurchaseInvoice",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InvoicePurchaseDate",
                table: "PurchaseInvoice",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "PurchaseInvoice",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payment",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "EditorName",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OriginItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "OrderType",
                table: "OrderInventory",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "OrderInventory",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "Oprationsys",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OprationName",
                table: "Oprationsys",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "OprationDescription",
                table: "Oprationsys",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IconClass",
                table: "Oprationsys",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ControllerName",
                table: "Oprationsys",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClassName",
                table: "Oprationsys",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ArabicOprationDescription",
                table: "Oprationsys",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MenuItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MembershipMovementOrder",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "MembershipMovementOrder",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "MembershipMovementOrder",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "EditorName",
                table: "MembershipMovementOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "MembershipMovement",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "MembershipMovement",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "EditorName",
                table: "MembershipMovement",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiscountDescription",
                table: "MembershipMovement",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Membership",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Membership",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "MemberLog",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "Member",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ssn",
                table: "Member",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber2",
                table: "Member",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldUnicode: false,
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber1",
                table: "Member",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldUnicode: false,
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Member",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Member",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateofBirth",
                table: "Member",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Massage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "Massage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendDate",
                table: "Massage",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Massage",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldUnicode: false,
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ItemMUO",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Item",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Item",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Item",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TypeMove",
                table: "InventoryMovement",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "InventoryMovement",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "FileData",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                table: "FileData",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "EntryAccounting",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EditorsUser",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Discount",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Discount",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Discount",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Device",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastSetDateTime",
                table: "Device",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "Device",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Device",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "CompanyInfo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "CompanyInfo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RateNumber",
                table: "CompanyInfo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber2",
                table: "CompanyInfo",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldUnicode: false,
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber1",
                table: "CompanyInfo",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldUnicode: false,
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                table: "CompanyInfo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CompanyInfo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Fax",
                table: "CompanyInfo",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldUnicode: false,
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "CompanyInfo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CompanyInfo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Payee",
                table: "Cheque",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "Cheque",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cheque",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "Cheque",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cash",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Bank",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Bank",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BackUp",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BackUp",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "BackUp",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataBaseName",
                table: "BackUp",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BackUpPath",
                table: "BackUp",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ActionLog",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostingDateTime",
                table: "ActionLog",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ActionLog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Vendor",
                newName: "AccountID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Vendor",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UnitItem",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StocktakingInventory",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "StocktakingInventoryId",
                table: "StockMovement",
                newName: "StocktakingInventoryID");

            migrationBuilder.RenameColumn(
                name: "ItemsId",
                table: "StockMovement",
                newName: "ItemsID");

            migrationBuilder.RenameColumn(
                name: "InventoryItemId",
                table: "StockMovement",
                newName: "InventoryItemID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StockMovement",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Service",
                newName: "ItemID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Service",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "SalesInvoice",
                newName: "VendorID");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "SalesInvoice",
                newName: "MemberID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SalesInvoice",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "PurchaseInvoice",
                newName: "VendorID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PurchaseInvoice",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "Payment",
                newName: "VendorID");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Payment",
                newName: "MemberID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payment",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OriginItem",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OrderInventory",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Oprationsys",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MenuItem",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "MemberShipMovementId",
                table: "MembershipMovementOrder",
                newName: "MemberShipMovementID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MembershipMovementOrder",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "MembershipId",
                table: "MembershipMovement",
                newName: "MembershipID");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "MembershipMovement",
                newName: "MemberID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MembershipMovement",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Membership",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "MemberLog",
                newName: "MemberID");

            migrationBuilder.RenameColumn(
                name: "DeviceId",
                table: "MemberLog",
                newName: "DeviceID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MemberLog",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "MemberFace",
                newName: "MemberID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MemberFace",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Ssn",
                table: "Member",
                newName: "SSN");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Member",
                newName: "AccountID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Member",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Fktable",
                table: "Massage",
                newName: "FKTable");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Massage",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "UnitItemId",
                table: "ItemMUO",
                newName: "UnitItemID");

            migrationBuilder.RenameColumn(
                name: "OriginItemId",
                table: "ItemMUO",
                newName: "OriginItemID");

            migrationBuilder.RenameColumn(
                name: "MenuItemId",
                table: "ItemMUO",
                newName: "MenuItemID");

            migrationBuilder.RenameColumn(
                name: "ItemsId",
                table: "ItemMUO",
                newName: "ItemsID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ItemMUO",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Item",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "SalesInvoiceId",
                table: "InventoryMovement",
                newName: "SalesInvoiceID");

            migrationBuilder.RenameColumn(
                name: "PurchaseInvoiceId",
                table: "InventoryMovement",
                newName: "PurchaseInvoiceID");

            migrationBuilder.RenameColumn(
                name: "OrderInventoryId",
                table: "InventoryMovement",
                newName: "OrderInventoryID");

            migrationBuilder.RenameColumn(
                name: "ItemsId",
                table: "InventoryMovement",
                newName: "ItemsID");

            migrationBuilder.RenameColumn(
                name: "InventoryItemId",
                table: "InventoryMovement",
                newName: "InventoryItemID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "InventoryMovement",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "InventoryItem",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Fktable",
                table: "FileData",
                newName: "FKTable");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FileData",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "EntryId",
                table: "EntryMovement",
                newName: "EntryID");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "EntryMovement",
                newName: "AccountID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EntryMovement",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EntryAccounting",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EditorsUser",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Discount",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Ip",
                table: "Device",
                newName: "IP");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Device",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CompanyInfo",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "Cheque",
                newName: "VendorID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Cheque",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Pcip",
                table: "Cash",
                newName: "PCIP");

            migrationBuilder.RenameColumn(
                name: "Btcash",
                table: "Cash",
                newName: "BTCash");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Cash",
                newName: "AccountID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Cash",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Iban",
                table: "Bank",
                newName: "IBAN");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Bank",
                newName: "AccountID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Bank",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BackUp",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BackUp",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "ActionLog",
                newName: "VendorID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ActionLog",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "ActionLog",
                newName: "UnitID");

            migrationBuilder.RenameColumn(
                name: "StocktakingInventoryId",
                table: "ActionLog",
                newName: "StocktakingInventoryID");

            migrationBuilder.RenameColumn(
                name: "StockMovementId",
                table: "ActionLog",
                newName: "StockMovementID");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "ActionLog",
                newName: "ServiceID");

            migrationBuilder.RenameColumn(
                name: "SalesInvoiceId",
                table: "ActionLog",
                newName: "SalesInvoiceID");

            migrationBuilder.RenameColumn(
                name: "PurchaseInvoiceId",
                table: "ActionLog",
                newName: "PurchaseInvoiceID");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "ActionLog",
                newName: "PaymentID");

            migrationBuilder.RenameColumn(
                name: "OriginId",
                table: "ActionLog",
                newName: "OriginID");

            migrationBuilder.RenameColumn(
                name: "OrderInventoryId",
                table: "ActionLog",
                newName: "OrderInventoryID");

            migrationBuilder.RenameColumn(
                name: "OprationId",
                table: "ActionLog",
                newName: "OprationID");

            migrationBuilder.RenameColumn(
                name: "MenuId",
                table: "ActionLog",
                newName: "MenuID");

            migrationBuilder.RenameColumn(
                name: "MembershipMovementOrderId",
                table: "ActionLog",
                newName: "MembershipMovementOrderID");

            migrationBuilder.RenameColumn(
                name: "MembershipMovementId",
                table: "ActionLog",
                newName: "MembershipMovementID");

            migrationBuilder.RenameColumn(
                name: "MembershipId",
                table: "ActionLog",
                newName: "MembershipID");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "ActionLog",
                newName: "MemberID");

            migrationBuilder.RenameColumn(
                name: "ItemsId",
                table: "ActionLog",
                newName: "ItemsID");

            migrationBuilder.RenameColumn(
                name: "InventoryMovementId",
                table: "ActionLog",
                newName: "InventoryMovementID");

            migrationBuilder.RenameColumn(
                name: "InventoryItemId",
                table: "ActionLog",
                newName: "InventoryItemID");

            migrationBuilder.RenameColumn(
                name: "EntryId",
                table: "ActionLog",
                newName: "EntryID");

            migrationBuilder.RenameColumn(
                name: "DiscountId",
                table: "ActionLog",
                newName: "DiscountID");

            migrationBuilder.RenameColumn(
                name: "ChequeId",
                table: "ActionLog",
                newName: "ChequeID");

            migrationBuilder.RenameColumn(
                name: "CashId",
                table: "ActionLog",
                newName: "CashID");

            migrationBuilder.RenameColumn(
                name: "BankId",
                table: "ActionLog",
                newName: "BankID");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "ActionLog",
                newName: "AccountID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ActionLog",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Account",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnitItem",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "StocktakingInventory",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StockMovement",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Service",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Service",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Service",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "SalesInvoice",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SalesInvoice",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "PurchaseInvoice",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PurchaseInvoice",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InvoicePurchaseDate",
                table: "PurchaseInvoice",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "PurchaseInvoice",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payment",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Payment",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "Payment",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "EditorName",
                table: "Payment",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OriginItem",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "OrderType",
                table: "OrderInventory",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "OrderInventory",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "Oprationsys",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OprationName",
                table: "Oprationsys",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "OprationDescription",
                table: "Oprationsys",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IconClass",
                table: "Oprationsys",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ControllerName",
                table: "Oprationsys",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClassName",
                table: "Oprationsys",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ArabicOprationDescription",
                table: "Oprationsys",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MenuItem",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MembershipMovementOrder",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "MembershipMovementOrder",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "MembershipMovementOrder",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "EditorName",
                table: "MembershipMovementOrder",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "MembershipMovement",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "MembershipMovement",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "EditorName",
                table: "MembershipMovement",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiscountDescription",
                table: "MembershipMovement",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Membership",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Membership",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "MemberLog",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "Member",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SSN",
                table: "Member",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber2",
                table: "Member",
                type: "varchar(13)",
                unicode: false,
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber1",
                table: "Member",
                type: "varchar(13)",
                unicode: false,
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Member",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Member",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateofBirth",
                table: "Member",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Massage",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "Massage",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendDate",
                table: "Massage",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Massage",
                type: "varchar(13)",
                unicode: false,
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ItemMUO",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Item",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Item",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Item",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TypeMove",
                table: "InventoryMovement",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "InventoryMovement",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "FileData",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                table: "FileData",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "EntryAccounting",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EditorsUser",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Discount",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Discount",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Discount",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Device",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastSetDateTime",
                table: "Device",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IP",
                table: "Device",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Device",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "CompanyInfo",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "CompanyInfo",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RateNumber",
                table: "CompanyInfo",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber2",
                table: "CompanyInfo",
                type: "varchar(25)",
                unicode: false,
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber1",
                table: "CompanyInfo",
                type: "varchar(25)",
                unicode: false,
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                table: "CompanyInfo",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CompanyInfo",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Fax",
                table: "CompanyInfo",
                type: "varchar(25)",
                unicode: false,
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "CompanyInfo",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CompanyInfo",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Payee",
                table: "Cheque",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "Cheque",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cheque",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "Cheque",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cash",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Bank",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Bank",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "BackUp",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BackUp",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "BackUp",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataBaseName",
                table: "BackUp",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BackUpPath",
                table: "BackUp",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "ActionLog",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostingDateTime",
                table: "ActionLog",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ActionLog",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
