using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class deletefingeremployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeFingerPrints");

            migrationBuilder.DropIndex(
                name: "IX_FingerPrint_MemberID",
                table: "FingerPrint");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "FingerPrint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MemberId",
                table: "FingerPrint",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "EmployeeFingerPrints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FingerPrint = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeFingerPrints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeFingerPrints_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FingerPrint_MemberID",
                table: "FingerPrint",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFingerPrints_EmployeeId",
                table: "EmployeeFingerPrints",
                column: "EmployeeId");
        }
    }
}
