using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fristrupminde_api.Migrations
{
    public partial class StatisticsInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "StatisticsDatas",
                columns: table => new
                {
                    ID = table.Column<byte[]>(nullable: false),
                    MilkLiter = table.Column<double>(nullable: false),
                    FatPercentage = table.Column<double>(nullable: false),
                    DateForData = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsDatas", x => x.ID);
                });

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
        }
    }
}
