using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HFA.Migrations
{
    /// <inheritdoc />
    public partial class image2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Images_HeroImageImageId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_HeroImageImageId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "HeroImageImageId",
                table: "Articles",
                newName: "HeroImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_HeroImageId",
                table: "Articles",
                column: "HeroImageId",
                unique: true,
                filter: "[HeroImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Images_HeroImageId",
                table: "Articles",
                column: "HeroImageId",
                principalTable: "Images",
                principalColumn: "ImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Images_HeroImageId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_HeroImageId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "HeroImageId",
                table: "Articles",
                newName: "HeroImageImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_HeroImageImageId",
                table: "Articles",
                column: "HeroImageImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Images_HeroImageImageId",
                table: "Articles",
                column: "HeroImageImageId",
                principalTable: "Images",
                principalColumn: "ImageId");
        }
    }
}
