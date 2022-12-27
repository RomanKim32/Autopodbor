using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Autopodbor_312.Migrations
{
    public partial class UploadFileNameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadFiles");

            migrationBuilder.CreateTable(
                name: "PortfolioNewsFiles",
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
                    table.PrimaryKey("PK_PortfolioNewsFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfolioNewsFiles_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PortfolioNewsFiles_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioNewsFiles_NewsId",
                table: "PortfolioNewsFiles",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioNewsFiles_PortfolioId",
                table: "PortfolioNewsFiles",
                column: "PortfolioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortfolioNewsFiles");

            migrationBuilder.CreateTable(
                name: "UploadFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NewsId = table.Column<int>(type: "integer", nullable: true),
                    Path = table.Column<string>(type: "text", nullable: true),
                    PortfolioId = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true)
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
    }
}
