using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HFA.Migrations
{
    /// <inheritdoc />
    public partial class removingcarouselpic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarouselPictures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarouselPictures",
                columns: table => new
                {
                    CarouselPictureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PictureLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarouselPictures", x => x.CarouselPictureId);
                });
        }
    }
}
