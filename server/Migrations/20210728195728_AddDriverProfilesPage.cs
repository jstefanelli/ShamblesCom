using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShamblesCom.Server.Migrations
{
    public partial class AddDriverProfilesPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DriverId = table.Column<int>(type: "INTEGER", nullable: false),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShortDescription = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImageData = table.Column<byte[]>(type: "BLOB", nullable: true),
                    ImageMimeType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Profiles_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_DriverId",
                table: "Profiles",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_SeasonId_DriverId",
                table: "Profiles",
                columns: new[] { "SeasonId", "DriverId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
