using Microsoft.EntityFrameworkCore.Migrations;

namespace Autopodbor_312.Migrations
{
    public partial class AddedLinksToResourcesInServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAdditional",
                table: "Services",
                newName: "IsAdditional");

            migrationBuilder.AddColumn<string>(
                name: "KeyForDescriptionInResourcesFiles",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyForNameInResourcesFiles",
                table: "Services",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyForDescriptionInResourcesFiles",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "KeyForNameInResourcesFiles",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "IsAdditional",
                table: "Services",
                newName: "isAdditional");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
