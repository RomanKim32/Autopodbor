using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class AddedMainPageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "MainPageFiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "MainPageFiles");
        }
    }
}
