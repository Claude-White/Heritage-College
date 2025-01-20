using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HFA.Migrations
{
    /// <inheritdoc />
    public partial class addlead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lead",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lead",
                table: "Articles");
        }
    }
}
