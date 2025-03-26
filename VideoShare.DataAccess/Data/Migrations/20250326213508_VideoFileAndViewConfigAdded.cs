using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoShare.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class VideoFileAndViewConfigAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoView_Video_VideoId",
                table: "VideoView");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoView_Video_VideoId",
                table: "VideoView",
                column: "VideoId",
                principalTable: "Video",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoView_Video_VideoId",
                table: "VideoView");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoView_Video_VideoId",
                table: "VideoView",
                column: "VideoId",
                principalTable: "Video",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
