using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class ChangedPropsInOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CarsBodyTypes_CarsBodyTypesId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CarsBrands_CarsBrandsId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CarsFuels_CarsFuelsId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CarsYears_CarsYearsId",
                table: "Orders");

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

            migrationBuilder.AlterColumn<int>(
                name: "CarsYearsId",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CarsFuelsId",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CarsBrandsId",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CarsBodyTypesId",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CarsBodyTypes_CarsBodyTypesId",
                table: "Orders",
                column: "CarsBodyTypesId",
                principalTable: "CarsBodyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CarsBrands_CarsBrandsId",
                table: "Orders",
                column: "CarsBrandsId",
                principalTable: "CarsBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CarsFuels_CarsFuelsId",
                table: "Orders",
                column: "CarsFuelsId",
                principalTable: "CarsFuels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CarsYears_CarsYearsId",
                table: "Orders",
                column: "CarsYearsId",
                principalTable: "CarsYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CarsBodyTypes_CarsBodyTypesId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CarsBrands_CarsBrandsId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CarsFuels_CarsFuelsId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CarsYears_CarsYearsId",
                table: "Orders");

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

            migrationBuilder.AlterColumn<int>(
                name: "CarsYearsId",
                table: "Orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarsFuelsId",
                table: "Orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarsBrandsId",
                table: "Orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarsBodyTypesId",
                table: "Orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CarsBodyTypes_CarsBodyTypesId",
                table: "Orders",
                column: "CarsBodyTypesId",
                principalTable: "CarsBodyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CarsBrands_CarsBrandsId",
                table: "Orders",
                column: "CarsBrandsId",
                principalTable: "CarsBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CarsFuels_CarsFuelsId",
                table: "Orders",
                column: "CarsFuelsId",
                principalTable: "CarsFuels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CarsYears_CarsYearsId",
                table: "Orders",
                column: "CarsYearsId",
                principalTable: "CarsYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
