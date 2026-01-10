using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddTheLoaiBlogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaTheLoai",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TheLoaiBlogs",
                columns: table => new
                {
                    MaTheLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTheLoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MauSac = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoLuongBlog = table.Column<int>(type: "int", nullable: false),
                    HienThi = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThuTu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheLoaiBlogs", x => x.MaTheLoai);
                });

            migrationBuilder.CreateIndex(
                name: "idx_blog_matheloai",
                table: "Blogs",
                column: "MaTheLoai");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_TheLoaiBlogs_MaTheLoai",
                table: "Blogs",
                column: "MaTheLoai",
                principalTable: "TheLoaiBlogs",
                principalColumn: "MaTheLoai",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_TheLoaiBlogs_MaTheLoai",
                table: "Blogs");

            migrationBuilder.DropTable(
                name: "TheLoaiBlogs");

            migrationBuilder.DropIndex(
                name: "idx_blog_matheloai",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MaTheLoai",
                table: "Blogs");
        }
    }
}
