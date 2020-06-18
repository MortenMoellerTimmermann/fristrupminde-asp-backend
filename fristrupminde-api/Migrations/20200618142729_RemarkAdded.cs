using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fristrupminde_api.Migrations
{
    public partial class RemarkAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "Remarks",
                columns: table => new
                {
                    ID = table.Column<byte[]>(nullable: false),
                    ProjectTaskID = table.Column<byte[]>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remarks", x => x.ID);
                });

            migrationBuilder.AddColumn<string>(
            name: "CreatedBy",
            table: "ProjectTasks",
            nullable: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "Remarks");


        }
    }
}
