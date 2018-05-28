using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Department.Data.Migrations
{
    public partial class addDepart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Departs",
                newName: "QQ");

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "Departs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "Departs");

            migrationBuilder.RenameColumn(
                name: "QQ",
                table: "Departs",
                newName: "Password");
        }
    }
}
