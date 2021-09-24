using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class FixSalaryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHoursLogs_Device_DeviceId",
                table: "WorkingHoursLogs");

            migrationBuilder.DropTable(
                name: "StaticAdjustments");

            migrationBuilder.DropTable(
                name: "WorkingHoursAdjustments");

            migrationBuilder.DropColumn(
                name: "IsWorkingHourAdjustment",
                table: "Adjustments");

            migrationBuilder.AlterColumn<long>(
                name: "DeviceId",
                table: "WorkingHoursLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "SalaryAdjustmentLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdjustmentAmmount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdjustmentId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryPaymentId = table.Column<long>(type: "bigint", nullable: true),
                    WorkingHoursLogId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryAdjustmentLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryAdjustmentLogs_Adjustments_AdjustmentId",
                        column: x => x.AdjustmentId,
                        principalTable: "Adjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryAdjustmentLogs_SalaryPayments_SalaryPaymentId",
                        column: x => x.SalaryPaymentId,
                        principalTable: "SalaryPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalaryAdjustmentLogs_WorkingHoursLogs_WorkingHoursLogId",
                        column: x => x.WorkingHoursLogId,
                        principalTable: "WorkingHoursLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdjustmentLogs_AdjustmentId",
                table: "SalaryAdjustmentLogs",
                column: "AdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdjustmentLogs_SalaryPaymentId",
                table: "SalaryAdjustmentLogs",
                column: "SalaryPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdjustmentLogs_WorkingHoursLogId",
                table: "SalaryAdjustmentLogs",
                column: "WorkingHoursLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHoursLogs_Device_DeviceId",
                table: "WorkingHoursLogs",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingHoursLogs_Device_DeviceId",
                table: "WorkingHoursLogs");

            migrationBuilder.DropTable(
                name: "SalaryAdjustmentLogs");

            migrationBuilder.AlterColumn<long>(
                name: "DeviceId",
                table: "WorkingHoursLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWorkingHourAdjustment",
                table: "Adjustments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "StaticAdjustments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdjustmentAmount = table.Column<double>(type: "float", nullable: false),
                    AdjustmentId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryPaymentId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaticAdjustments_Adjustments_AdjustmentId",
                        column: x => x.AdjustmentId,
                        principalTable: "Adjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaticAdjustments_SalaryPayments_SalaryPaymentId",
                        column: x => x.SalaryPaymentId,
                        principalTable: "SalaryPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingHoursAdjustments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdjustmentAmmount = table.Column<double>(type: "float", nullable: false),
                    AdjustmentId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryPaymentId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<double>(type: "float", nullable: true),
                    WorkingHoursLogId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingHoursAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingHoursAdjustments_Adjustments_AdjustmentId",
                        column: x => x.AdjustmentId,
                        principalTable: "Adjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingHoursAdjustments_SalaryPayments_SalaryPaymentId",
                        column: x => x.SalaryPaymentId,
                        principalTable: "SalaryPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkingHoursAdjustments_WorkingHoursLogs_WorkingHoursLogId",
                        column: x => x.WorkingHoursLogId,
                        principalTable: "WorkingHoursLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaticAdjustments_AdjustmentId",
                table: "StaticAdjustments",
                column: "AdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticAdjustments_SalaryPaymentId",
                table: "StaticAdjustments",
                column: "SalaryPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustments_AdjustmentId",
                table: "WorkingHoursAdjustments",
                column: "AdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustments_SalaryPaymentId",
                table: "WorkingHoursAdjustments",
                column: "SalaryPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustments_WorkingHoursLogId",
                table: "WorkingHoursAdjustments",
                column: "WorkingHoursLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingHoursLogs_Device_DeviceId",
                table: "WorkingHoursLogs",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
