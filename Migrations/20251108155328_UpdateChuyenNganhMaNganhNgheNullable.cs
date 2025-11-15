using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChuyenNganhMaNganhNgheNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChuyenNganhs_NganhNghes_MaNganhNghe",
                table: "ChuyenNganhs");

            migrationBuilder.AlterColumn<int>(
                name: "MaNganhNghe",
                table: "ChuyenNganhs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ChuyenNganhs_NganhNghes_MaNganhNghe",
                table: "ChuyenNganhs",
                column: "MaNganhNghe",
                principalTable: "NganhNghes",
                principalColumn: "MaNganhNghe",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChuyenNganhs_NganhNghes_MaNganhNghe",
                table: "ChuyenNganhs");

            migrationBuilder.AlterColumn<int>(
                name: "MaNganhNghe",
                table: "ChuyenNganhs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChuyenNganhs_NganhNghes_MaNganhNghe",
                table: "ChuyenNganhs",
                column: "MaNganhNghe",
                principalTable: "NganhNghes",
                principalColumn: "MaNganhNghe",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
