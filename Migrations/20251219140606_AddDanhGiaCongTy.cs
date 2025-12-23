using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddDanhGiaCongTy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhGiaCongTys",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTinUngTuyen = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaNhaTuyenDung = table.Column<int>(type: "int", nullable: false),
                    DiemMinhBachLuong = table.Column<int>(type: "int", nullable: false),
                    DiemTocDoPhanHoi = table.Column<int>(type: "int", nullable: false),
                    DiemTonTrongUngVien = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAnDanh = table.Column<bool>(type: "bit", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhanHoi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayPhanHoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoLuotThich = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaCongTys", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK_DanhGiaCongTys_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DanhGiaCongTys_NhaTuyenDungs_MaNhaTuyenDung",
                        column: x => x.MaNhaTuyenDung,
                        principalTable: "NhaTuyenDungs",
                        principalColumn: "MaNhaTuyenDung");
                    table.ForeignKey(
                        name: "FK_DanhGiaCongTys_TinUngTuyens_MaTinUngTuyen",
                        column: x => x.MaTinUngTuyen,
                        principalTable: "TinUngTuyens",
                        principalColumn: "MaTinUngTuyen");
                });

            migrationBuilder.CreateIndex(
                name: "idx_danhgiacongty_matinungtuyen",
                table: "DanhGiaCongTys",
                column: "MaTinUngTuyen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_danhgiacongty_nhatuyendung",
                table: "DanhGiaCongTys",
                column: "MaNhaTuyenDung");

            migrationBuilder.CreateIndex(
                name: "idx_danhgiacongty_userid",
                table: "DanhGiaCongTys",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DanhGiaCongTys");
        }
    }
}
