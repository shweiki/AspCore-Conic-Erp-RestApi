using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class HumanResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Ssn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vaccine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
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
                    Tax = table.Column<double>(type: "float", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    WorkingHoursId = table.Column<long>(type: "bigint", nullable: false),
                    AdjustmentId = table.Column<int>(type: "int", nullable: false),
                    SalaryPaymentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingHoursAdjustments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeFingerPrints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FingerPrint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "WorkingHoursLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingHoursLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingHoursLogs_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingHoursLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Adjustments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentAmount = table.Column<double>(type: "float", nullable: false),
                    AdjustmentPercentage = table.Column<double>(type: "float", nullable: false),
                    WorkingHoursAdjustmentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adjustments_WorkingHoursAdjustments_WorkingHoursAdjustmentId",
                        column: x => x.WorkingHoursAdjustmentId,
                        principalTable: "WorkingHoursAdjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalaryPayments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    GrossSalary = table.Column<double>(type: "float", nullable: false),
                    NetSalary = table.Column<double>(type: "float", nullable: false),
                    SalaryPeriod = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkingHoursAdjustmentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryPayments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryPayments_WorkingHoursAdjustments_WorkingHoursAdjustmentId",
                        column: x => x.WorkingHoursAdjustmentId,
                        principalTable: "WorkingHoursAdjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Adjustments_WorkingHoursAdjustmentId",
                table: "Adjustments",
                column: "WorkingHoursAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFingerPrints_EmployeeId",
                table: "EmployeeFingerPrints",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AccountId",
                table: "Employees",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryPayments_EmployeeId",
                table: "SalaryPayments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryPayments_WorkingHoursAdjustmentId",
                table: "SalaryPayments",
                column: "WorkingHoursAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursAdjustmentWorkingHoursLog_WorkingHoursLogsId",
                table: "WorkingHoursAdjustmentWorkingHoursLog",
                column: "WorkingHoursLogsId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursLogs_DeviceId",
                table: "WorkingHoursLogs",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursLogs_EmployeeId",
                table: "WorkingHoursLogs",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adjustments");

            migrationBuilder.DropTable(
                name: "EmployeeFingerPrints");

            migrationBuilder.DropTable(
                name: "SalaryPayments");

            migrationBuilder.DropTable(
                name: "WorkingHoursAdjustmentWorkingHoursLog");

            migrationBuilder.DropTable(
                name: "WorkingHoursAdjustments");

            migrationBuilder.DropTable(
                name: "WorkingHoursLogs");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
