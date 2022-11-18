using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Autopodbor_312.Migrations
{
    public partial class AddedOrderModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarsBodyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BodyType = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarsBodyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarsBrands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarsBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarsEngines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EngineSize = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarsEngines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarsFuels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FuelsType = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarsFuels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarsYears",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ManufacturesYear = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarsYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServicesId = table.Column<int>(nullable: false),
                    OrderTime = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CarsBrandsId = table.Column<int>(nullable: false),
                    CarsYearsId = table.Column<int>(nullable: false),
                    CarsBodyTypesId = table.Column<int>(nullable: false),
                    CarsEnginesId = table.Column<int>(nullable: false),
                    CarsFuelsId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_CarsBodyTypes_CarsBodyTypesId",
                        column: x => x.CarsBodyTypesId,
                        principalTable: "CarsBodyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_CarsBrands_CarsBrandsId",
                        column: x => x.CarsBrandsId,
                        principalTable: "CarsBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_CarsEngines_CarsEnginesId",
                        column: x => x.CarsEnginesId,
                        principalTable: "CarsEngines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_CarsFuels_CarsFuelsId",
                        column: x => x.CarsFuelsId,
                        principalTable: "CarsFuels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_CarsYears_CarsYearsId",
                        column: x => x.CarsYearsId,
                        principalTable: "CarsYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CarsBodyTypesId",
                table: "Orders",
                column: "CarsBodyTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CarsBrandsId",
                table: "Orders",
                column: "CarsBrandsId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CarsEnginesId",
                table: "Orders",
                column: "CarsEnginesId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CarsFuelsId",
                table: "Orders",
                column: "CarsFuelsId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CarsYearsId",
                table: "Orders",
                column: "CarsYearsId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ServicesId",
                table: "Orders",
                column: "ServicesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "CarsBodyTypes");

            migrationBuilder.DropTable(
                name: "CarsBrands");

            migrationBuilder.DropTable(
                name: "CarsEngines");

            migrationBuilder.DropTable(
                name: "CarsFuels");

            migrationBuilder.DropTable(
                name: "CarsYears");
        }
    }
}
