using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.DDD.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeCode",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "TypeCode",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
