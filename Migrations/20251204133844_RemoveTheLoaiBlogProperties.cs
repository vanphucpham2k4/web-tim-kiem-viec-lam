using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTheLoaiBlogProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "TheLoaiBlogs");

            migrationBuilder.DropColumn(
                name: "MauSac",
                table: "TheLoaiBlogs");

            migrationBuilder.DropColumn(
                name: "MoTa",
                table: "TheLoaiBlogs");

            migrationBuilder.DropColumn(
                name: "SoLuongBlog",
                table: "TheLoaiBlogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "TheLoaiBlogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MauSac",
                table: "TheLoaiBlogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MoTa",
                table: "TheLoaiBlogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoLuongBlog",
                table: "TheLoaiBlogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
