using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class staticAdjustment2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaticAdjustments_Adjustments_AdjustmentId1",
                table: "StaticAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StaticAdjustments_SalaryPayments_SalaryPaymentId1",
                table: "StaticAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_StaticAdjustments_AdjustmentId1",
                table: "StaticAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_StaticAdjustments_SalaryPaymentId1",
                table: "StaticAdjustments");

            migrationBuilder.DropColumn(
                name: "AdjustmentId1",
                table: "StaticAdjustments");

            migrationBuilder.DropColumn(
                name: "SalaryPaymentId1",
                table: "StaticAdjustments");

            migrationBuilder.AlterColumn<long>(
                name: "SalaryPaymentId",
                table: "StaticAdjustments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AdjustmentId",
                table: "StaticAdjustments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_StaticAdjustments_AdjustmentId",
                table: "StaticAdjustments",
                column: "AdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticAdjustments_SalaryPaymentId",
                table: "StaticAdjustments",
                column: "SalaryPaymentId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaticAdjustments_Adjustments_AdjustmentId",
                table: "StaticAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StaticAdjustments_SalaryPayments_SalaryPaymentId",
                table: "StaticAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_StaticAdjustments_AdjustmentId",
                table: "StaticAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_StaticAdjustments_SalaryPaymentId",
                table: "StaticAdjustments");

            migrationBuilder.AlterColumn<string>(
                name: "SalaryPaymentId",
                table: "StaticAdjustments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdjustmentId",
                table: "StaticAdjustments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AdjustmentId1",
                table: "StaticAdjustments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SalaryPaymentId1",
                table: "StaticAdjustments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaticAdjustments_AdjustmentId1",
                table: "StaticAdjustments",
                column: "AdjustmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_StaticAdjustments_SalaryPaymentId1",
                table: "StaticAdjustments",
                column: "SalaryPaymentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StaticAdjustments_Adjustments_AdjustmentId1",
                table: "StaticAdjustments",
                column: "AdjustmentId1",
                principalTable: "Adjustments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StaticAdjustments_SalaryPayments_SalaryPaymentId1",
                table: "StaticAdjustments",
                column: "SalaryPaymentId1",
                principalTable: "SalaryPayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
