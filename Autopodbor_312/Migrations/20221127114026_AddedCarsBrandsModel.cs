using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Autopodbor_312.Migrations
{
    public partial class AddedCarsBrandsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarsBrandsModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    CarsBrandsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarsBrandsModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarsBrandsModels_CarsBrands_CarsBrandsId",
                        column: x => x.CarsBrandsId,
                        principalTable: "CarsBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarsBrandsModels_CarsBrandsId",
                table: "CarsBrandsModels",
                column: "CarsBrandsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarsBrandsModels");
        }
    }
}
