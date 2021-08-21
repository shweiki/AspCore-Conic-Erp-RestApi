using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class changenamecolum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Totalrestitution",
                table: "CashPools",
                newName: "TotalRestitution");

            migrationBuilder.AlterColumn<string>(
                name: "Fktable",
                table: "CashPools",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CashPools",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CashPools");

            migrationBuilder.RenameColumn(
                name: "TotalRestitution",
                table: "CashPools",
                newName: "Totalrestitution");

            migrationBuilder.AlterColumn<long>(
                name: "Fktable",
                table: "CashPools",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
