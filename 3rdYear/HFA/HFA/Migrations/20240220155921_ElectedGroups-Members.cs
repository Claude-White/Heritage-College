using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HFA.Migrations
{
    /// <inheritdoc />
    public partial class ElectedGroupsMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElectedGroups",
                columns: table => new
                {
                    GroupId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectedGroups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "ElectedMembers",
                columns: table => new
                {
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberFirstName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    MemberLastName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    MemberGroup = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectedMembers", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_ElectedMembers_ElectedGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ElectedGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElectedMembers_GroupId",
                table: "ElectedMembers",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectedMembers");

            migrationBuilder.DropTable(
                name: "ElectedGroups");
        }
    }
}
