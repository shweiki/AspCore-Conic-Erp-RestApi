using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class Changesome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         
            migrationBuilder.DropTable(
                name: "MemberLog");


            migrationBuilder.CreateTable(
                name: "DeviceLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceLog_Device",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FingerPrint",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Str = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FingerPrint", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLog_DeviceID",
                table: "DeviceLog",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLog_MemberID",
                table: "DeviceLog",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLog_Status",
                table: "DeviceLog",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FingerPrint_MemberID",
                table: "FingerPrint",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceLog");

            migrationBuilder.DropTable(
                name: "FingerPrint");


      
        
            migrationBuilder.CreateTable(
                name: "MemberLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberLog_Device",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberLog_Member",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateIndex(
                name: "IX_MemberLog_DeviceID",
                table: "MemberLog",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberLog_MemberID",
                table: "MemberLog",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberLog_Status",
                table: "MemberLog",
                column: "Status");
        }
    }
}
