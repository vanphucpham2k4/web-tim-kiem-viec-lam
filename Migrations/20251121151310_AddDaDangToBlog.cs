using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class AddDaDangToBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DaDang",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaDang",
                table: "Blogs");
        }
    }
}
