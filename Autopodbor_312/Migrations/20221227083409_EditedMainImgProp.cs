using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class EditedMainImgProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImagePath",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "MainImagePath",
                table: "News");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainImagePath",
                table: "Portfolio",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImagePath",
                table: "News",
                type: "text",
                nullable: true);
        }
    }
}
