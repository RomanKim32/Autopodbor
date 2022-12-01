using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class AddPropInServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditinalServiceText",
                table: "Services");

            migrationBuilder.AddColumn<bool>(
                name: "isAdditional",
                table: "Services",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdditional",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "AdditinalServiceText",
                table: "Services",
                type: "text",
                nullable: true);
        }
    }
}
