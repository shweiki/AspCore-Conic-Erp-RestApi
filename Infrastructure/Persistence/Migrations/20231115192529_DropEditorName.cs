using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class DropEditorName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditorsUser");

            migrationBuilder.DropColumn(
                name: "EditorName",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "EditorName",
                table: "MembershipMovement");

            migrationBuilder.RenameColumn(
                name: "EditorName",
                table: "Receive",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "EditorName",
                table: "MembershipMovementOrder",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "EditorName",
                table: "CashPool",
                newName: "LastModifiedBy");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Receive",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Receive",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Receive",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Receive",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "MembershipMovementOrder",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MembershipMovementOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MembershipMovementOrder",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "MembershipMovementOrder",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "CashPool",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CashPool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CashPool",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "CashPool",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Receive");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Receive");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Receive");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Receive");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "MembershipMovementOrder");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MembershipMovementOrder");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MembershipMovementOrder");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "MembershipMovementOrder");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "CashPool");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CashPool");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CashPool");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "CashPool");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Receive",
                newName: "EditorName");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "MembershipMovementOrder",
                newName: "EditorName");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "CashPool",
                newName: "EditorName");

            migrationBuilder.AddColumn<string>(
                name: "EditorName",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditorName",
                table: "MembershipMovement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EditorsUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditorsUser", x => x.Id);
                });
        }
    }
}
