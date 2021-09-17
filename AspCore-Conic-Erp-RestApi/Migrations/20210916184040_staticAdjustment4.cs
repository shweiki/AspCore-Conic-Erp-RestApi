using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class staticAdjustment4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaticAdjustments_Adjustments_AdjustmentId",
                table: "StaticAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StaticAdjustments_SalaryPayments_SalaryPaymentId",
                table: "StaticAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHoursAdjustments_Adjustments_AdjustmentId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.AlterColumn<long>(
                name: "AdjustmentId",
                table: "WorkingHoursAdjustments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SalaryPaymentId",
                table: "StaticAdjustments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AdjustmentId",
                table: "StaticAdjustments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StaticAdjustments_Adjustments_AdjustmentId",
                table: "StaticAdjustments",
                column: "AdjustmentId",
                principalTable: "Adjustments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StaticAdjustments_SalaryPayments_SalaryPaymentId",
                table: "StaticAdjustments",
                column: "SalaryPaymentId",
                principalTable: "SalaryPayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHoursAdjustments_Adjustments_AdjustmentId",
                table: "WorkingHoursAdjustments",
                column: "AdjustmentId",
                principalTable: "Adjustments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaticAdjustments_Adjustments_AdjustmentId",
                table: "StaticAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StaticAdjustments_SalaryPayments_SalaryPaymentId",
                table: "StaticAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHoursAdjustments_Adjustments_AdjustmentId",
                table: "WorkingHoursAdjustments");

            migrationBuilder.AlterColumn<long>(
                name: "AdjustmentId",
                table: "WorkingHoursAdjustments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "SalaryPaymentId",
                table: "StaticAdjustments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "AdjustmentId",
                table: "StaticAdjustments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_StaticAdjustments_Adjustments_AdjustmentId",
                table: "StaticAdjustments",
                column: "AdjustmentId",
                principalTable: "Adjustments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StaticAdjustments_SalaryPayments_SalaryPaymentId",
                table: "StaticAdjustments",
                column: "SalaryPaymentId",
                principalTable: "SalaryPayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHoursAdjustments_Adjustments_AdjustmentId",
                table: "WorkingHoursAdjustments",
                column: "AdjustmentId",
                principalTable: "Adjustments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
