using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tod.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndTeamTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Todos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    teamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    teamInvitationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    teamLeader = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.teamId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    teamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                    table.ForeignKey(
                        name: "FK_Users_Teams_teamId",
                        column: x => x.teamId,
                        principalTable: "Teams",
                        principalColumn: "teamId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_userId",
                table: "Todos",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_teamLeader",
                table: "Teams",
                column: "teamLeader");

            migrationBuilder.CreateIndex(
                name: "IX_Users_teamId",
                table: "Users",
                column: "teamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Users_userId",
                table: "Todos",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Users_teamLeader",
                table: "Teams",
                column: "teamLeader",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Users_userId",
                table: "Todos");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Users_teamLeader",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Todos_userId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Todos");
        }
    }
}
