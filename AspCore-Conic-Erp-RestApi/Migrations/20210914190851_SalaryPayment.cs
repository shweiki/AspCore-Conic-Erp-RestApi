using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class SalaryPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryPayments_Employees_EmployeeId",
                table: "SalaryPayments");

            migrationBuilder.DropIndex(
                name: "IX_SalaryPayments_EmployeeId",
                table: "SalaryPayments");

            migrationBuilder.CreateTable(
                name: "EmployeeSalaryPayment",
                columns: table => new
                {
                    EmployeesId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryPaymentsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSalaryPayment", x => new { x.EmployeesId, x.SalaryPaymentsId });
                    table.ForeignKey(
                        name: "FK_EmployeeSalaryPayment_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSalaryPayment_SalaryPayments_SalaryPaymentsId",
                        column: x => x.SalaryPaymentsId,
                        principalTable: "SalaryPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryPayment_SalaryPaymentsId",
                table: "EmployeeSalaryPayment",
                column: "SalaryPaymentsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSalaryPayment");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryPayments_EmployeeId",
                table: "SalaryPayments",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryPayments_Employees_EmployeeId",
                table: "SalaryPayments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
