using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Autopodbor_312.Migrations
{
    public partial class AddedMainPageandMAinPageFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainPage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Banner = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainPage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MainPageFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Path = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    MainPageId = table.Column<int>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainPageFiles");

            migrationBuilder.DropTable(
                name: "MainPage");
        }
    }
}
