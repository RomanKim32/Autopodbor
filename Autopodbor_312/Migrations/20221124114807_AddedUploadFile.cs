using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Autopodbor_312.Migrations
{
    public partial class AddedUploadFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "Video",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "News");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Portfolio",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "News",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "UploadFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Path = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    NewsId = table.Column<int>(nullable: true),
                    PortfolioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadFiles_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UploadFiles_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_NewsId",
                table: "UploadFiles",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_PortfolioId",
                table: "UploadFiles",
                column: "PortfolioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadFiles");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "News");

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Portfolio",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "Portfolio",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Orders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "News",
                type: "text",
                nullable: true);
        }
    }
}
