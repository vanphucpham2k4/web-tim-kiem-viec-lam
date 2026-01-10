using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddChuyenNganhTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChuyenNganhKhac",
                table: "UngViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaChuyenNganh",
                table: "UngViens",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChuyenNganhs",
                columns: table => new
                {
                    MaChuyenNganh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChuyenNganh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNganhNghe = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuyenNganhs", x => x.MaChuyenNganh);
                    table.ForeignKey(
                        name: "FK_ChuyenNganhs_NganhNghes_MaNganhNghe",
                        column: x => x.MaNganhNghe,
                        principalTable: "NganhNghes",
                        principalColumn: "MaNganhNghe",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "idx_ungvien_chuyennganh",
                table: "UngViens",
                column: "MaChuyenNganh");

            migrationBuilder.CreateIndex(
                name: "idx_chuyennganh_nganhnghe",
                table: "ChuyenNganhs",
                column: "MaNganhNghe");

            migrationBuilder.AddForeignKey(
                name: "FK_UngViens_ChuyenNganhs_MaChuyenNganh",
                table: "UngViens",
                column: "MaChuyenNganh",
                principalTable: "ChuyenNganhs",
                principalColumn: "MaChuyenNganh",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UngViens_ChuyenNganhs_MaChuyenNganh",
                table: "UngViens");

            migrationBuilder.DropTable(
                name: "ChuyenNganhs");

            migrationBuilder.DropIndex(
                name: "idx_ungvien_chuyennganh",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "ChuyenNganhKhac",
                table: "UngViens");

            migrationBuilder.DropColumn(
                name: "MaChuyenNganh",
                table: "UngViens");
        }
    }
}
