using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Department.Data.Migrations
{
    public partial class addApplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Count = table.Column<int>(nullable: false),
                    DepartID = table.Column<long>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Grade = table.Column<string>(nullable: false),
                    Institute = table.Column<string>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_DepartID",
                table: "Applications",
                column: "DepartID");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Grade",
                table: "Applications",
                column: "Grade");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Institute",
                table: "Applications",
                column: "Institute");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
