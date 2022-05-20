using Microsoft.EntityFrameworkCore.Migrations;

namespace ShamblesCom.Server.Migrations
{
    public partial class AddSeasonOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "Seasons",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Options",
                table: "Seasons");
        }
    }
}
