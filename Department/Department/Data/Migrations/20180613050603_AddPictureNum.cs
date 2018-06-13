using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Department.Data.Migrations
{
    public partial class AddPictureNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Departs");

            migrationBuilder.DropColumn(
                name: "Actpictures",
                table: "Activities");

            migrationBuilder.AddColumn<int>(
                name: "PictureNum",
                table: "Departs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Noticetime",
                table: "Activities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureNum",
                table: "Departs");

            migrationBuilder.DropColumn(
                name: "Noticetime",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Departs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Actpictures",
                table: "Activities",
                nullable: true);
        }
    }
}
