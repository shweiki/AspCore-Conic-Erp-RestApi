﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class changekeycoltoemailsentreporttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Keys",
                table: "Reports",
                newName: "EmailSent");

            migrationBuilder.AddColumn<string>(
                name: "AutoSent",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoSent",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "EmailSent",
                table: "Reports",
                newName: "Keys");
        }
    }
}
