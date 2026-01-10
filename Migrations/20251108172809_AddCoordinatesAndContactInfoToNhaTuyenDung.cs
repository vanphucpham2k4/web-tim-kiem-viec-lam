using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddCoordinatesAndContactInfoToNhaTuyenDung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailNguoiDaiDien",
                table: "NhaTuyenDungs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "NhaTuyenDungs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "NhaTuyenDungs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoaiNguoiDaiDien",
                table: "NhaTuyenDungs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailNguoiDaiDien",
                table: "NhaTuyenDungs");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "NhaTuyenDungs");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "NhaTuyenDungs");

            migrationBuilder.DropColumn(
                name: "SoDienThoaiNguoiDaiDien",
                table: "NhaTuyenDungs");
        }
    }
}
