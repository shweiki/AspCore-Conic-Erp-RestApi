using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class staticAdjustment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "SalaryPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsStaticAdjustment",
                table: "Adjustments",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                    AdjustmentPercentage = table.Column<double>(type: "float", nullable: false),
                    AdjustmentId = table.Column<int>(type: "int", nullable: false),
                    SalaryPaymentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryPaymentId1 = table.Column<long>(type: "bigint", nullable: true),
                    AdjustmentId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaticAdjustments_Adjustments_AdjustmentId1",
                        column: x => x.AdjustmentId1,
                        principalTable: "Adjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaticAdjustments_SalaryPayments_SalaryPaymentId1",
                        column: x => x.SalaryPaymentId1,
                        principalTable: "SalaryPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaticAdjustments_AdjustmentId1",
                table: "StaticAdjustments",
                column: "AdjustmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_StaticAdjustments_SalaryPaymentId1",
                table: "StaticAdjustments",
                column: "SalaryPaymentId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaticAdjustments");

            migrationBuilder.DropColumn(
                name: "status",
                table: "SalaryPayments");

            migrationBuilder.DropColumn(
                name: "IsStaticAdjustment",
                table: "Adjustments");

            migrationBuilder.DropColumn(
                name: "IsWorkingHourAdjustment",
                table: "Adjustments");
        }
    }
}
