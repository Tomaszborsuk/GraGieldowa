using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraGieldowa.Model.Migrations
{
    public partial class StockModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClosedPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(type: "TEXT", nullable: true),
                    StockName = table.Column<string>(type: "TEXT", nullable: true),
                    OpenPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosePrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    NetProfit = table.Column<decimal>(type: "TEXT", nullable: false),
                    ROIPercent = table.Column<decimal>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    OpenDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(type: "TEXT", nullable: true),
                    StockName = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    OpenDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenPositions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClosedPositions");

            migrationBuilder.DropTable(
                name: "OpenPositions");
        }
    }
}
