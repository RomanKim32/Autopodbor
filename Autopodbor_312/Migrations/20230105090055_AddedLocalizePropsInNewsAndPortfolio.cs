using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class AddedLocalizePropsInNewsAndPortfolio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "News");

            migrationBuilder.AddColumn<string>(
                name: "BodyKy",
                table: "Portfolio",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BodyRu",
                table: "Portfolio",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameKy",
                table: "Portfolio",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameRu",
                table: "Portfolio",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BodyKy",
                table: "News",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BodyRu",
                table: "News",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameKy",
                table: "News",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameRu",
                table: "News",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyKy",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "BodyRu",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "NameKy",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "NameRu",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "BodyKy",
                table: "News");

            migrationBuilder.DropColumn(
                name: "BodyRu",
                table: "News");

            migrationBuilder.DropColumn(
                name: "NameKy",
                table: "News");

            migrationBuilder.DropColumn(
                name: "NameRu",
                table: "News");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Portfolio",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Portfolio",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "News",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "News",
                type: "text",
                nullable: true);
        }
    }
}
