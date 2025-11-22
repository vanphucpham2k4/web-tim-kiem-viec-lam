using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPermalinkAutoToBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPermalinkAuto",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValue: true);

            // Cập nhật tất cả các bản ghi hiện có thành true (mặc định)
            migrationBuilder.Sql(@"
                UPDATE Blogs 
                SET IsPermalinkAuto = 1 
                WHERE IsPermalinkAuto = 0
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPermalinkAuto",
                table: "Blogs");
        }
    }
}
