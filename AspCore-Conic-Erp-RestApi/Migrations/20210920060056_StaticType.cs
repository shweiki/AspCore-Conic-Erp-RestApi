using Microsoft.EntityFrameworkCore.Migrations;

namespace AspCore_Conic_Erp_RestApi.Migrations
{
    public partial class StaticType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberFace");

            migrationBuilder.DropColumn(
                name: "AdjustmentPercentage",
                table: "StaticAdjustments");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StaticAdjustments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "StaticAdjustments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FingerPrints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Str = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FingerPrints", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FingerPrints");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StaticAdjustments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "StaticAdjustments");

            migrationBuilder.AddColumn<double>(
                name: "AdjustmentPercentage",
                table: "StaticAdjustments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "MemberFace",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaceLength = table.Column<int>(type: "int", nullable: false),
                    FaceStr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberFace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberFace_Member",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberFace_MemberID",
                table: "MemberFace",
                column: "MemberId");
        }
    }
}
