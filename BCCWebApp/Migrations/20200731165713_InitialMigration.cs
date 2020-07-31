using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BCCWebApp.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwitchUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    ProfileImageUrl = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TwitchUserId = table.Column<string>(nullable: true),
                    NaviName = table.Column<string>(maxLength: 4, nullable: true),
                    NaviCode = table.Column<string>(maxLength: 24, nullable: true),
                    Wins = table.Column<int>(nullable: false),
                    Battles = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decks_TwitchUsers_TwitchUserId",
                        column: x => x.TwitchUserId,
                        principalTable: "TwitchUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    TwitchUserId = table.Column<string>(nullable: false),
                    TotalWins = table.Column<int>(nullable: false),
                    TotalBattles = table.Column<int>(nullable: false),
                    CurrentDeckId = table.Column<int>(nullable: true),
                    TorunamentRegistered = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.TwitchUserId);
                    table.ForeignKey(
                        name: "FK_Users_Decks_CurrentDeckId",
                        column: x => x.CurrentDeckId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_TwitchUsers_TwitchUserId",
                        column: x => x.TwitchUserId,
                        principalTable: "TwitchUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Decks_TwitchUserId",
                table: "Decks",
                column: "TwitchUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentDeckId",
                table: "Users",
                column: "CurrentDeckId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "TwitchUsers");
        }
    }
}
