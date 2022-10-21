using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.DDD.Infrastructure.Migrations
{
    public partial class UpdateTabla_Article_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeCode",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeCode",
                table: "Articles");
        }
    }
}
