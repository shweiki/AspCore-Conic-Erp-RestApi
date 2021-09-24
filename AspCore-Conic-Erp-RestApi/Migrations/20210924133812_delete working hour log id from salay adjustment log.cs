using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class deleteworkinghourlogidfromsalayadjustmentlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryAdjustmentLogs_WorkingHoursLogs_WorkingHoursLogId",
                table: "SalaryAdjustmentLogs");

            migrationBuilder.DropIndex(
                name: "IX_SalaryAdjustmentLogs_WorkingHoursLogId",
                table: "SalaryAdjustmentLogs");

            migrationBuilder.DropColumn(
                name: "WorkingHoursLogId",
                table: "SalaryAdjustmentLogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "WorkingHoursLogId",
                table: "SalaryAdjustmentLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdjustmentLogs_WorkingHoursLogId",
                table: "SalaryAdjustmentLogs",
                column: "WorkingHoursLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryAdjustmentLogs_WorkingHoursLogs_WorkingHoursLogId",
                table: "SalaryAdjustmentLogs",
                column: "WorkingHoursLogId",
                principalTable: "WorkingHoursLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
