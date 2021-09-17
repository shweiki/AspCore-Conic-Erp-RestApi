using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class workingadjustmentrelationfix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SalaryPaymentId",
                table: "WorkingHoursAdjustments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkingHoursLogId",
                table: "WorkingHoursAdjustments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustments_SalaryPaymentId",
                table: "WorkingHoursAdjustments",
                column: "SalaryPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustments_WorkingHoursLogId",
                table: "WorkingHoursAdjustments",
                column: "WorkingHoursLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHoursAdjustments_SalaryPayments_SalaryPaymentId",
                table: "WorkingHoursAdjustments",
                column: "SalaryPaymentId",
                principalTable: "SalaryPayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHoursAdjustments_WorkingHoursLogs_WorkingHoursLogId",
                table: "WorkingHoursAdjustments",
                column: "WorkingHoursLogId",
                principalTable: "WorkingHoursLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHoursAdjustments_SalaryPayments_SalaryPaymentId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHoursAdjustments_WorkingHoursLogs_WorkingHoursLogId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_WorkingHoursAdjustments_SalaryPaymentId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_WorkingHoursAdjustments_WorkingHoursLogId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropColumn(
                name: "SalaryPaymentId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropColumn(
                name: "WorkingHoursLogId",
                table: "WorkingHoursAdjustments");
        }
    }
}
