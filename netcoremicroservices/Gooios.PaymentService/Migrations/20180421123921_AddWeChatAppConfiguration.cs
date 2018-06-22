using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.PaymentService.Migrations
{
    public partial class AddWeChatAppConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wechat_app_configurations",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    app_id = table.Column<string>(maxLength: 80, nullable: false),
                    app_secret = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    key = table.Column<string>(maxLength: 80, nullable: false),
                    mch_id = table.Column<string>(maxLength: 80, nullable: false),
                    notify_url = table.Column<string>(maxLength: 200, nullable: false),
                    organization_id = table.Column<string>(maxLength: 80, nullable: false),
                    sslcert_password = table.Column<string>(maxLength: 80, nullable: false),
                    sslcert_path = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wechat_app_configurations", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wechat_app_configurations");
        }
    }
}
