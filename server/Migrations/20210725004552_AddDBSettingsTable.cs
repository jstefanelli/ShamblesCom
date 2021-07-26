using Microsoft.EntityFrameworkCore.Migrations;

namespace ShamblesCom.Server.Migrations
{
    public partial class AddDBSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LiveRaceId = table.Column<int>(type: "INTEGER", nullable: true),
                    LiveStreamLinks = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentHomeSeasonId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settings_Races_LiveRaceId",
                        column: x => x.LiveRaceId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Settings_Seasons_CurrentHomeSeasonId",
                        column: x => x.CurrentHomeSeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Settings_CurrentHomeSeasonId",
                table: "Settings",
                column: "CurrentHomeSeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_LiveRaceId",
                table: "Settings",
                column: "LiveRaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
