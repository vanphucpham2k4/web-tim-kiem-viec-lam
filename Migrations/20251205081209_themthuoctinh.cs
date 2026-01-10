using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class themthuoctinh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaTruong",
                table: "TinUngTuyens",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "idx_tinungtuyen_truong",
                table: "TinUngTuyens",
                column: "MaTruong");

            migrationBuilder.AddForeignKey(
                name: "FK_TinUngTuyens_TruongDaiHocs_MaTruong",
                table: "TinUngTuyens",
                column: "MaTruong",
                principalTable: "TruongDaiHocs",
                principalColumn: "MaTruong",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TinUngTuyens_TruongDaiHocs_MaTruong",
                table: "TinUngTuyens");

            migrationBuilder.DropIndex(
                name: "idx_tinungtuyen_truong",
                table: "TinUngTuyens");

            migrationBuilder.DropColumn(
                name: "MaTruong",
                table: "TinUngTuyens");
        }
    }
}
