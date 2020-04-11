using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fristrupminde_api.Migrations
{
    public partial class AddedGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "UserID",
                table: "ProjectTaskUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProjectTaskID",
                table: "ProjectTaskUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProjectTaskUserID",
                table: "ProjectTaskUsers",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<byte[]>(
                name: "ID",
                table: "ProjectTasks",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<byte[]>(
                name: "Id",
                table: "ApplicationUsers",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<byte[]>(
                name: "ID",
                table: "Animals",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "ProjectTaskUsers",
                nullable: true,
                oldClrType: typeof(byte[]));

            migrationBuilder.AlterColumn<string>(
                name: "ProjectTaskID",
                table: "ProjectTaskUsers",
                nullable: true,
                oldClrType: typeof(byte[]));

            migrationBuilder.AlterColumn<string>(
                name: "ProjectTaskUserID",
                table: "ProjectTaskUsers",
                nullable: false,
                oldClrType: typeof(byte[]));

            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "ProjectTasks",
                nullable: false,
                oldClrType: typeof(byte[]));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ApplicationUsers",
                nullable: false,
                oldClrType: typeof(byte[]));

            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "Animals",
                nullable: false,
                oldClrType: typeof(byte[]));
        }
    }
}
