using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddDanhGiaCongTyLikeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhGiaCongTyLikes",
                columns: table => new
                {
                    MaLike = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDanhGia = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayLike = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaCongTyLikes", x => x.MaLike);
                    table.ForeignKey(
                        name: "FK_DanhGiaCongTyLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGiaCongTyLikes_DanhGiaCongTys_MaDanhGia",
                        column: x => x.MaDanhGia,
                        principalTable: "DanhGiaCongTys",
                        principalColumn: "MaDanhGia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_danhgiacongtylike_unique",
                table: "DanhGiaCongTyLikes",
                columns: new[] { "MaDanhGia", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaCongTyLikes_UserId",
                table: "DanhGiaCongTyLikes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DanhGiaCongTyLikes");
        }
    }
}
