using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspCoreConicErpRestApi.Migrations
{
    /// <inheritdoc />
    public partial class addST9tobillofentry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "BillOfEnterys",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ST9",
                table: "BillOfEnterys",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ST9",
                table: "BillOfEnterys");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FakeDate",
                table: "BillOfEnterys",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
