using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Department.Data.Migrations
{
    public partial class RemoveDnames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartName",
                table: "DtoMMappings");

            migrationBuilder.DropColumn(
                name: "DName",
                table: "Applications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartName",
                table: "DtoMMappings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DName",
                table: "Applications",
                nullable: true);
        }
    }
}
