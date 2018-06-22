using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.VerificationService.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "verifications",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    biz_code = table.Column<int>(nullable: false),
                    code = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    expired_on = table.Column<DateTime>(nullable: false),
                    is_suspend = table.Column<bool>(nullable: false),
                    is_used = table.Column<bool>(nullable: false),
                    last_upd_on = table.Column<DateTime>(nullable: false),
                    to = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_verifications", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "verifications");
        }
    }
}
