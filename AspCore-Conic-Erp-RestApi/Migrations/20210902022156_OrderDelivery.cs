using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class OrderDelivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderDelivery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellingPrice = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPrime = table.Column<bool>(type: "bit", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    DriverId = table.Column<long>(type: "bigint", nullable: false),
                    AreaId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDelivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDelivery_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDelivery_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDelivery_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDelivery_AreaId",
                table: "OrderDelivery",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDelivery_DriverId",
                table: "OrderDelivery",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDelivery_VendorId",
                table: "OrderDelivery",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDelivery");
        }
    }
}
