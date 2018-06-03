using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Department.Data.Migrations
{
    public partial class DtoAAddStudentID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StudentID",
                table: "DtoAMappings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_DtoAMappings_StudentID",
                table: "DtoAMappings",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DtoAMappings_StudentID",
                table: "DtoAMappings");

            migrationBuilder.DropColumn(
                name: "StudentID",
                table: "DtoAMappings");
        }
    }
}
