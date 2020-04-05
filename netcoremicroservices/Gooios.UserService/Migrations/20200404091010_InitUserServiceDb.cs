using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gooios.UserService.Migrations
{
    public partial class InitUserServiceDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cookapp_partiner_loginusers",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    partner_auth_code = table.Column<string>(maxLength: 200, nullable: true),
                    partner_access_token = table.Column<string>(maxLength: 2000, nullable: true),
                    expired_in = table.Column<int>(nullable: false),
                    refresh_token = table.Column<string>(maxLength: 1000, nullable: true),
                    scope = table.Column<string>(maxLength: 200, nullable: true),
                    union_id = table.Column<string>(maxLength: 1000, nullable: true),
                    login_channel = table.Column<int>(nullable: false),
                    partner_key = table.Column<string>(maxLength: 200, nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(maxLength: 80, nullable: false),
                    updated_on = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cookapp_partiner_loginusers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cookapp_users",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    user_name = table.Column<string>(maxLength: 80, nullable: false),
                    password = table.Column<string>(maxLength: 200, nullable: false),
                    mobile = table.Column<string>(maxLength: 20, nullable: true),
                    email = table.Column<string>(maxLength: 200, nullable: true),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(maxLength: 80, nullable: false),
                    updated_on = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cookapp_users", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cookapp_partiner_loginusers");

            migrationBuilder.DropTable(
                name: "cookapp_users");
        }
    }
}
