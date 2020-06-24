using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fristrupminde_api.Migrations
{
    public partial class RemarkCreatedDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
            name: "CreatedBy",
            table: "Remarks",
            nullable: false);

            migrationBuilder.AddColumn<DateTime>(
            name: "Created",
            table: "Remarks",
            nullable: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Remarks");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Remarks");
        }
    }
}
