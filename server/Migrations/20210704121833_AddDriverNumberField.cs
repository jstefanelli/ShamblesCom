using Microsoft.EntityFrameworkCore.Migrations;

namespace ShamblesCom.Server.Migrations
{
    public partial class AddDriverNumberField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_Tracks_Trackid",
                table: "Races");

            migrationBuilder.RenameColumn(
                name: "Trackid",
                table: "Races",
                newName: "TrackId");

            migrationBuilder.RenameIndex(
                name: "IX_Races_Trackid",
                table: "Races",
                newName: "IX_Races_TrackId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Races",
                type: "TEXT",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Drivers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Tracks_TrackId",
                table: "Races",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_Tracks_TrackId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "TrackId",
                table: "Races",
                newName: "Trackid");

            migrationBuilder.RenameIndex(
                name: "IX_Races_TrackId",
                table: "Races",
                newName: "IX_Races_Trackid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Races",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 2147483647);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Tracks_Trackid",
                table: "Races",
                column: "Trackid",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
