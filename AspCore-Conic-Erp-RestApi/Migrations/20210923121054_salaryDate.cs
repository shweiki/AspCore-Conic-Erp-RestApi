using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class salaryDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "SalaryPayments",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "SalaryPeriod",
                table: "SalaryPayments",
                newName: "SalaryTo");

            migrationBuilder.AddColumn<DateTime>(
                name: "SalaryFrom",
                table: "SalaryPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryFrom",
                table: "SalaryPayments");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "SalaryPayments",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "SalaryTo",
                table: "SalaryPayments",
                newName: "SalaryPeriod");
        }
    }
}
