using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicareer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMaUngVienToUserIdInTinUngTuyen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TinUngTuyens",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TinUngTuyens_UserId",
                table: "TinUngTuyens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TinUngTuyens_AspNetUsers_UserId",
                table: "TinUngTuyens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TinUngTuyens_AspNetUsers_UserId",
                table: "TinUngTuyens");

            migrationBuilder.DropIndex(
                name: "IX_TinUngTuyens_UserId",
                table: "TinUngTuyens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TinUngTuyens");
        }
    }
}
