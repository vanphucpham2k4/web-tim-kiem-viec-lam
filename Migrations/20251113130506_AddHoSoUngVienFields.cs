using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddHoSoUngVienFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CVFile",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChungChi",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HienThiCongKhai",
                table: "UngViens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "HocVanChiTiet",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KinhNghiemChiTiet",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyNangChiTiet",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkBehance",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkGitHub",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkPortfolio",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MoTaBanThan",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MucLuongKyVong",
                table: "UngViens",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MucTieuNgheNghiep",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiLamViecMongMuon",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrangThaiTimViec",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViTriMongMuon",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVFile",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "ChungChi",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "HienThiCongKhai",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "HocVanChiTiet",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "KinhNghiemChiTiet",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "KyNangChiTiet",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "LinkBehance",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "LinkGitHub",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "LinkPortfolio",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "MoTaBanThan",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "MucLuongKyVong",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "MucTieuNgheNghiep",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "NoiLamViecMongMuon",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "TrangThaiTimViec",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "ViTriMongMuon",
                table: "UngViens");
        }
    }
}
