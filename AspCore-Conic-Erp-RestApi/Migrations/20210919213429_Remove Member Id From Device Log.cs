using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class RemoveMemberIdFromDeviceLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeviceLog_MemberID",
                table: "DeviceLog");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "DeviceLog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MemberId",
                table: "DeviceLog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLog_MemberID",
                table: "DeviceLog",
                column: "MemberId");
        }
    }
}
