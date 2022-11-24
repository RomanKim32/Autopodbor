using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Autopodbor_312.Migrations
{
    public partial class RemovedCarsEngineModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CarsEngines_CarsEnginesId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "CarsEngines");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CarsEnginesId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CarsEnginesId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "CarsEnginesId",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CarsEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EngineSize = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarsEngines", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CarsEnginesId",
                table: "Orders",
                column: "CarsEnginesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CarsEngines_CarsEnginesId",
                table: "Orders",
                column: "CarsEnginesId",
                principalTable: "CarsEngines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
