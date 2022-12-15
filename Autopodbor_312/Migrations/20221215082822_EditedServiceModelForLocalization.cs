using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class EditedServiceModelForLocalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "KeyForDescriptionInResourcesFiles",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "KeyForNameInResourcesFiles",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionKy",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameKy",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameRu",
                table: "Services",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionKy",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "NameKy",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "NameRu",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyForDescriptionInResourcesFiles",
                table: "Services",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyForNameInResourcesFiles",
                table: "Services",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Services",
                type: "text",
                nullable: true);
        }
    }
}
