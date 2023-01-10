using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class AddedPropsInPortfolioToFilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarsBodyTypesId",
                table: "Portfolio",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CarsBrandsId",
                table: "Portfolio",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CarsBrandsModelId",
                table: "Portfolio",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFieldInspection",
                table: "Portfolio",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_CarsBodyTypesId",
                table: "Portfolio",
                column: "CarsBodyTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_CarsBrandsId",
                table: "Portfolio",
                column: "CarsBrandsId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_CarsBrandsModelId",
                table: "Portfolio",
                column: "CarsBrandsModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_CarsBodyTypes_CarsBodyTypesId",
                table: "Portfolio",
                column: "CarsBodyTypesId",
                principalTable: "CarsBodyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_CarsBrands_CarsBrandsId",
                table: "Portfolio",
                column: "CarsBrandsId",
                principalTable: "CarsBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_CarsBrandsModels_CarsBrandsModelId",
                table: "Portfolio",
                column: "CarsBrandsModelId",
                principalTable: "CarsBrandsModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfolio_CarsBodyTypes_CarsBodyTypesId",
                table: "Portfolio");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolio_CarsBrands_CarsBrandsId",
                table: "Portfolio");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolio_CarsBrandsModels_CarsBrandsModelId",
                table: "Portfolio");

            migrationBuilder.DropIndex(
                name: "IX_Portfolio_CarsBodyTypesId",
                table: "Portfolio");

            migrationBuilder.DropIndex(
                name: "IX_Portfolio_CarsBrandsId",
                table: "Portfolio");

            migrationBuilder.DropIndex(
                name: "IX_Portfolio_CarsBrandsModelId",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "CarsBodyTypesId",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "CarsBrandsId",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "CarsBrandsModelId",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "IsFieldInspection",
                table: "Portfolio");
        }
    }
}
