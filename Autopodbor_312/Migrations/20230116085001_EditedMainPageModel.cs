using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Autopodbor_312.Migrations
{
    public partial class EditedMainPageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainPageFiles");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MainPage");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MainPage");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "MainPage");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionKy",
                table: "MainPage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "MainPage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "MainPage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleKy",
                table: "MainPage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleRu",
                table: "MainPage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionKy",
                table: "MainPage");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "MainPage");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "MainPage");

            migrationBuilder.DropColumn(
                name: "TitleKy",
                table: "MainPage");

            migrationBuilder.DropColumn(
                name: "TitleRu",
                table: "MainPage");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MainPage",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MainPage",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MainPage",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MainPageFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MainPageId = table.Column<int>(type: "integer", nullable: true),
                    Path = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainPageFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainPageFiles_MainPage_MainPageId",
                        column: x => x.MainPageId,
                        principalTable: "MainPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainPageFiles_MainPageId",
                table: "MainPageFiles",
                column: "MainPageId");
        }
    }
}
