using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class AddPropertyInServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "AdditinalServiceText",
                table: "Services",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditinalServiceText",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Services",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
