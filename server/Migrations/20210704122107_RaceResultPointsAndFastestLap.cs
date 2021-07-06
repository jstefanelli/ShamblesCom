using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShamblesCom.Server.Migrations
{
    public partial class RaceResultPointsAndFastestLap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "FastestLap",
                table: "RaceResults",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "RaceResults",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "RaceResults");

            migrationBuilder.AlterColumn<bool>(
                name: "FastestLap",
                table: "RaceResults",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT");
        }
    }
}
