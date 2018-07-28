using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.AuthorizationService.Data.Migrations.ApplicationDb
{
    public partial class AddUserSessions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "applet_user_sessions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_on = table.Column<DateTime>(nullable: false),
                    expired_on = table.Column<DateTime>(nullable: false),
                    gooios_session_key = table.Column<string>(maxLength: 1000, nullable: false),
                    open_id = table.Column<string>(maxLength: 500, nullable: true),
                    session_key = table.Column<string>(maxLength: 1000, nullable: false),
                    user_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applet_user_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "applet_users",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    application_id = table.Column<string>(maxLength: 200, nullable: false),
                    channel = table.Column<int>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    updated_on = table.Column<DateTime>(nullable: false),
                    nickname = table.Column<string>(maxLength: 200, nullable: false),
                    open_id = table.Column<string>(maxLength: 500, nullable: true),
                    organization_id = table.Column<string>(maxLength: 80, nullable: false),
                    user_id = table.Column<string>(maxLength: 80, nullable: false),
                    user_portrait = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applet_users", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applet_user_sessions");

            migrationBuilder.DropTable(
                name: "applet_users");
        }
    }
}
