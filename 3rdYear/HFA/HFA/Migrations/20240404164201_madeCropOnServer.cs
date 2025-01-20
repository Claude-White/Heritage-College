using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HFA.Migrations
{
    /// <inheritdoc />
    public partial class madeCropOnServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CropData",
                table: "Images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CropData",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
