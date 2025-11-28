using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDaDuyetFromBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaDuyet",
                table: "Blogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DaDuyet",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
