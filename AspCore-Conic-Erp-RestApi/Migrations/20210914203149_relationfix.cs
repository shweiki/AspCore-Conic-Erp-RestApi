using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class relationfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adjustments_WorkingHoursAdjustments_WorkingHoursAdjustmentId",
                table: "Adjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_SalaryPayments_WorkingHoursAdjustments_WorkingHoursAdjustmentId",
                table: "SalaryPayments");

            migrationBuilder.DropTable(
                name: "EmployeeSalaryPayment");

            migrationBuilder.DropTable(
                name: "WorkingHoursAdjustmentWorkingHoursLog");

            migrationBuilder.DropIndex(
                name: "IX_SalaryPayments_WorkingHoursAdjustmentId",
                table: "SalaryPayments");

            migrationBuilder.DropIndex(
                name: "IX_Adjustments_WorkingHoursAdjustmentId",
                table: "Adjustments");

            migrationBuilder.DropColumn(
                name: "WorkingHoursAdjustmentId",
                table: "SalaryPayments");

            migrationBuilder.DropColumn(
                name: "WorkingHoursAdjustmentId",
                table: "Adjustments");

            migrationBuilder.RenameColumn(
                name: "WorkingHoursId",
                table: "WorkingHoursAdjustments",
                newName: "WorkingHoursLogId");

            migrationBuilder.AddColumn<long>(
                name: "AdjustmentId1",
                table: "WorkingHoursAdjustments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SalaryPaymentId1",
                table: "WorkingHoursAdjustments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustments_AdjustmentId1",
                table: "WorkingHoursAdjustments",
                column: "AdjustmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustments_SalaryPaymentId1",
                table: "WorkingHoursAdjustments",
                column: "SalaryPaymentId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustments_WorkingHoursLogId",
                table: "WorkingHoursAdjustments",
                column: "WorkingHoursLogId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHoursAdjustments_Adjustments_AdjustmentId1",
                table: "WorkingHoursAdjustments",
                column: "AdjustmentId1",
                principalTable: "Adjustments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHoursAdjustments_SalaryPayments_SalaryPaymentId1",
                table: "WorkingHoursAdjustments",
                column: "SalaryPaymentId1",
                principalTable: "SalaryPayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHoursAdjustments_WorkingHoursLogs_WorkingHoursLogId",
                table: "WorkingHoursAdjustments",
                column: "WorkingHoursLogId",
                principalTable: "WorkingHoursLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryPayments_Employees_EmployeeId",
                table: "SalaryPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHoursAdjustments_Adjustments_AdjustmentId1",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHoursAdjustments_SalaryPayments_SalaryPaymentId1",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHoursAdjustments_WorkingHoursLogs_WorkingHoursLogId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_WorkingHoursAdjustments_AdjustmentId1",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_WorkingHoursAdjustments_SalaryPaymentId1",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_WorkingHoursAdjustments_WorkingHoursLogId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_SalaryPayments_EmployeeId",
                table: "SalaryPayments");

            migrationBuilder.DropColumn(
                name: "AdjustmentId1",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropColumn(
                name: "SalaryPaymentId1",
                table: "WorkingHoursAdjustments");

            migrationBuilder.RenameColumn(
                name: "WorkingHoursLogId",
                table: "WorkingHoursAdjustments",
                newName: "WorkingHoursId");

            migrationBuilder.AddColumn<long>(
                name: "WorkingHoursAdjustmentId",
                table: "SalaryPayments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkingHoursAdjustmentId",
                table: "Adjustments",
                type: "bigint",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "WorkingHoursAdjustmentWorkingHoursLog",
                columns: table => new
                {
                    WorkingHoursAdjusmentsId = table.Column<long>(type: "bigint", nullable: false),
                    WorkingHoursLogsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingHoursAdjustmentWorkingHoursLog", x => new { x.WorkingHoursAdjusmentsId, x.WorkingHoursLogsId });
                    table.ForeignKey(
                        name: "FK_WorkingHoursAdjustmentWorkingHoursLog_WorkingHoursAdjustments_WorkingHoursAdjusmentsId",
                        column: x => x.WorkingHoursAdjusmentsId,
                        principalTable: "WorkingHoursAdjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingHoursAdjustmentWorkingHoursLog_WorkingHoursLogs_WorkingHoursLogsId",
                        column: x => x.WorkingHoursLogsId,
                        principalTable: "WorkingHoursLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryPayments_WorkingHoursAdjustmentId",
                table: "SalaryPayments",
                column: "WorkingHoursAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Adjustments_WorkingHoursAdjustmentId",
                table: "Adjustments",
                column: "WorkingHoursAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryPayment_SalaryPaymentsId",
                table: "EmployeeSalaryPayment",
                column: "SalaryPaymentsId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustmentWorkingHoursLog_WorkingHoursLogsId",
                table: "WorkingHoursAdjustmentWorkingHoursLog",
                column: "WorkingHoursLogsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adjustments_WorkingHoursAdjustments_WorkingHoursAdjustmentId",
                table: "Adjustments",
                column: "WorkingHoursAdjustmentId",
                principalTable: "WorkingHoursAdjustments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryPayments_WorkingHoursAdjustments_WorkingHoursAdjustmentId",
                table: "SalaryPayments",
                column: "WorkingHoursAdjustmentId",
                principalTable: "WorkingHoursAdjustments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
