using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddTrangThaiToTinTuyenDung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "TinTuyenDungs",
                type: "nvarchar(max)",
                nullable: true);

            // Cập nhật các bản ghi hiện có thành "Dang tuyen" nếu null
            migrationBuilder.Sql(@"
                UPDATE TinTuyenDungs 
                SET TrangThai = 'Dang tuyen' 
                WHERE TrangThai IS NULL
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "TinTuyenDungs");
        }
    }
}
