using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddViecLamDaLuuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ViecLamDaLuus",
                columns: table => new
                {
                    MaViecLamDaLuu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaTinTuyenDung = table.Column<int>(type: "int", nullable: false),
                    NgayLuu = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViecLamDaLuus", x => x.MaViecLamDaLuu);
                    table.ForeignKey(
                        name: "FK_ViecLamDaLuus_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ViecLamDaLuus_TinTuyenDungs_MaTinTuyenDung",
                        column: x => x.MaTinTuyenDung,
                        principalTable: "TinTuyenDungs",
                        principalColumn: "MaTinTuyenDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_vieclamdaluu_matintuyendung",
                table: "ViecLamDaLuus",
                column: "MaTinTuyenDung");

            migrationBuilder.CreateIndex(
                name: "idx_vieclamdaluu_userid",
                table: "ViecLamDaLuus",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "idx_vieclamdaluu_userid_matintuyendung",
                table: "ViecLamDaLuus",
                columns: new[] { "UserId", "MaTinTuyenDung" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViecLamDaLuus");
        }
    }
}
