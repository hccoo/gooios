using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.LogService.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "system_logs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    app_service_name = table.Column<string>(maxLength: 80, nullable: false),
                    application_key = table.Column<string>(maxLength: 80, nullable: false),
                    biz_data = table.Column<string>(maxLength: 2000, nullable: false),
                    caller_application_key = table.Column<string>(maxLength: 80, nullable: true),
                    created_on = table.Column<DateTime>(nullable: false),
                    exception = table.Column<string>(maxLength: 2000, nullable: true),
                    level = table.Column<int>(nullable: false),
                    log_thread = table.Column<int>(nullable: false),
                    log_time = table.Column<DateTime>(nullable: false),
                    operation = table.Column<string>(maxLength: 200, nullable: false),
                    return_value = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_logs", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "system_logs");
        }
    }
}
