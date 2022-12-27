using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class AddedMainImgProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainImagePath",
                table: "Portfolio",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImagePath",
                table: "News",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImagePath",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "MainImagePath",
                table: "News");
        }
    }
}
