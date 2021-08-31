using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ReceiveId",
                table: "ActionLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Receives",
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
                    table.PrimaryKey("PK_Receives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receives_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receives_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receives_MemberId",
                table: "Receives",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_VendorId",
                table: "Receives",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receives");

            migrationBuilder.DropColumn(
                name: "ReceiveId",
                table: "ActionLog");
        }
    }
}
