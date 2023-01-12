using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class AddedCarsModelPropInOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameRu",
                table: "Services",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "NameKy",
                table: "Services",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionRu",
                table: "Services",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionKy",
                table: "Services",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "CarsBrandsModelsId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CarsBrandsModelsId",
                table: "Orders",
                column: "CarsBrandsModelsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CarsBrandsModels_CarsBrandsModelsId",
                table: "Orders",
                column: "CarsBrandsModelsId",
                principalTable: "CarsBrandsModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CarsBrandsModels_CarsBrandsModelsId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CarsBrandsModelsId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CarsBrandsModelsId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "NameRu",
                table: "Services",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameKy",
                table: "Services",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionRu",
                table: "Services",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionKy",
                table: "Services",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
