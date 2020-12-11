using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class addDefultData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PCIP",
                table: "Cash",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BTCash",
                table: "Cash",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "ID", "Code", "Description", "IsPrime", "Name", "Status", "Type" },
                values: new object[,]
                {
                    { 3L, "", "", false, "اشتراكات", 0, "InCome" },
                    { 4L, "", "", false, "مقبوضات", 0, "InCome" },
                    { 5L, "", "", false, "خزينة 1", 0, "Cash" },
                    { 6L, "", "", false, "زبون نقدي", 0, "Vendor" }
                });

            migrationBuilder.InsertData(
                table: "InventoryItem",
                columns: new[] { "ID", "Description", "IsPrime", "Name", "Status" },
                values: new object[] { 1, "", true, "المخزن 1", 0 });

            migrationBuilder.InsertData(
                table: "Cash",
                columns: new[] { "ID", "AccountID", "BTCash", "Description", "IsPrime", "Name", "PCIP", "Status" },
                values: new object[] { 2L, 5L, "Com3", "", true, "خزينة 1", "192.168.1.0", 0 });

            migrationBuilder.InsertData(
                table: "Vendor",
                columns: new[] { "ID", "AccountID", "Address", "CreditLimit", "Description", "Email", "Fax", "IsPrime", "Name", "PhoneNumber1", "PhoneNumber2", "Status", "Type" },
                values: new object[] { 2L, 6L, null, 0.0, "", null, null, true, "زبون نقدي", null, null, 0, "Customer" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Cash",
                keyColumn: "ID",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "InventoryItem",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vendor",
                keyColumn: "ID",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 6L);

            migrationBuilder.AlterColumn<double>(
                name: "PCIP",
                table: "Cash",
                type: "float",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "BTCash",
                table: "Cash",
                type: "float",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
