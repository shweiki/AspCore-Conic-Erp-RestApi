﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace RestApi.Migrations
{
    public partial class EmpAcc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeUserId",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeUserId",
                table: "Employees");
        }
    }
}