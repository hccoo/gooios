using Microsoft.EntityFrameworkCore.Migrations;

namespace Gooios.UserService.Migrations
{
    public partial class AddServicerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "servicer_id",
                table: "cookapp_users",
                maxLength: 80,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "servicer_id",
                table: "cookapp_users");
        }
    }
}
