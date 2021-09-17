using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class staticAdjustment3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "AdjustmentId1",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropColumn(
                name: "SalaryPaymentId1",
                table: "WorkingHoursAdjustments");

            migrationBuilder.AlterColumn<long>(
                name: "WorkingHoursLogId",
                table: "WorkingHoursAdjustments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "SalaryPaymentId",
                table: "WorkingHoursAdjustments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AdjustmentId",
                table: "WorkingHoursAdjustments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustments_AdjustmentId",
                table: "WorkingHoursAdjustments",
                column: "AdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustments_SalaryPaymentId",
                table: "WorkingHoursAdjustments",
                column: "SalaryPaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHoursAdjustments_Adjustments_AdjustmentId",
                table: "WorkingHoursAdjustments",
                column: "AdjustmentId",
                principalTable: "Adjustments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_WorkingHoursAdjustments_Adjustments_AdjustmentId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHoursAdjustments_SalaryPayments_SalaryPaymentId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHoursAdjustments_WorkingHoursLogs_WorkingHoursLogId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_WorkingHoursAdjustments_AdjustmentId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_WorkingHoursAdjustments_SalaryPaymentId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.AlterColumn<long>(
                name: "WorkingHoursLogId",
                table: "WorkingHoursAdjustments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SalaryPaymentId",
                table: "WorkingHoursAdjustments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdjustmentId",
                table: "WorkingHoursAdjustments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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
    }
}
