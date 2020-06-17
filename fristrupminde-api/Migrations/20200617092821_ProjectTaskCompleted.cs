using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fristrupminde_api.Migrations
{
    public partial class ProjectTaskCompleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "ProjectTasks",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "CompletedBy",
                table: "ProjectTasks",
                nullable: true);
        }
    }
}
