using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class EditedPortfolioPropsToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "CarsBrandsModelId",
                table: "Portfolio",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CarsBrandsId",
                table: "Portfolio",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CarsBodyTypesId",
                table: "Portfolio",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_CarsBodyTypes_CarsBodyTypesId",
                table: "Portfolio",
                column: "CarsBodyTypesId",
                principalTable: "CarsBodyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_CarsBrands_CarsBrandsId",
                table: "Portfolio",
                column: "CarsBrandsId",
                principalTable: "CarsBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_CarsBrandsModels_CarsBrandsModelId",
                table: "Portfolio",
                column: "CarsBrandsModelId",
                principalTable: "CarsBrandsModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.AlterColumn<int>(
                name: "CarsBrandsModelId",
                table: "Portfolio",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarsBrandsId",
                table: "Portfolio",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarsBodyTypesId",
                table: "Portfolio",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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
    }
}
