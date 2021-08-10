﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShamblesCom.Server.Migrations
{
    public partial class AddTeamImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Teams",
                type: "BLOB",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "Teams",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "Teams");
        }
    }
}
