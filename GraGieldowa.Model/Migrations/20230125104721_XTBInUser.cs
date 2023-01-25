using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraGieldowa.Model.Migrations
{
    public partial class XTBInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "XTBPassword",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XTBUserId",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XTBPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "XTBUserId",
                table: "Users");
        }
    }
}
