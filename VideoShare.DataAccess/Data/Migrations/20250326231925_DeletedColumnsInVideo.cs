using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoShare.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeletedColumnsInVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Video");

            migrationBuilder.DropColumn(
                name: "Contents",
                table: "Video");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Video",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Contents",
                table: "Video",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
