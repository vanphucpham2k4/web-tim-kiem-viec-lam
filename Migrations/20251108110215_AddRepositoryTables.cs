using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddRepositoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoaiCongViecs",
                columns: table => new
                {
                    MaLoaiCongViec = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiCongViec = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuongViTri = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiCongViecs", x => x.MaLoaiCongViec);
                });

            migrationBuilder.CreateTable(
                name: "NganhNghes",
                columns: table => new
                {
                    MaNganhNghe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNganhNghe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuongCongViec = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NganhNghes", x => x.MaNganhNghe);
                });

            migrationBuilder.CreateTable(
                name: "TinTuyenDungs",
                columns: table => new
                {
                    MaTinTuyenDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenViecLam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CongTy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NganhNghe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NganhNgheChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiCongViec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KinhNghiem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViTri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgoaiNgu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TuKhoa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KyNang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YeuCau = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MucLuongThapNhat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MucLuongCaoNhat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QuyenLoi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NguoiLienHe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailLienHe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDTLienHe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhThanhPho = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhuongXa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChiLamViec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnhVanPhong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoLuongUngTuyen = table.Column<int>(type: "int", nullable: true),
                    NgayDang = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HanNop = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinTuyenDungs", x => x.MaTinTuyenDung);
                });

            migrationBuilder.CreateTable(
                name: "TinUngTuyens",
                columns: table => new
                {
                    MaTinUngTuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViTriUngTuyen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CongTy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaTinTuyenDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiXuLy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkCV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayUngTuyen = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinUngTuyens", x => x.MaTinUngTuyen);
                });

            migrationBuilder.CreateTable(
                name: "TruongDaiHocs",
                columns: table => new
                {
                    MaTruong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTruong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruongDaiHocs", x => x.MaTruong);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoaiCongViecs");

            migrationBuilder.DropTable(
                name: "NganhNghes");

            migrationBuilder.DropTable(
                name: "TinTuyenDungs");

            migrationBuilder.DropTable(
                name: "TinUngTuyens");

            migrationBuilder.DropTable(
                name: "TruongDaiHocs");
        }
    }
}
