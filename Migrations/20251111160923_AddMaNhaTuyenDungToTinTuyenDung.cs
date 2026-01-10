using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddMaNhaTuyenDungToTinTuyenDung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaNhaTuyenDung",
                table: "TinTuyenDungs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "idx_tintuyendung_nhatuyendung",
                table: "TinTuyenDungs",
                column: "MaNhaTuyenDung");

            migrationBuilder.AddForeignKey(
                name: "FK_TinTuyenDungs_NhaTuyenDungs_MaNhaTuyenDung",
                table: "TinTuyenDungs",
                column: "MaNhaTuyenDung",
                principalTable: "NhaTuyenDungs",
                principalColumn: "MaNhaTuyenDung",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TinTuyenDungs_NhaTuyenDungs_MaNhaTuyenDung",
                table: "TinTuyenDungs");

            migrationBuilder.DropIndex(
                name: "idx_tintuyendung_nhatuyendung",
                table: "TinTuyenDungs");

            migrationBuilder.DropColumn(
                name: "MaNhaTuyenDung",
                table: "TinTuyenDungs");
        }
    }
}
