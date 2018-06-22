using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.OrganizationService.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    area = table.Column<string>(maxLength: 20, nullable: false),
                    certificate_no = table.Column<string>(maxLength: 80, nullable: false),
                    city = table.Column<string>(maxLength: 20, nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    full_name = table.Column<string>(maxLength: 200, nullable: false),
                    introduction = table.Column<string>(maxLength: 4000, nullable: true),
                    is_suspend = table.Column<bool>(nullable: false),
                    updated_by = table.Column<string>(maxLength: 80, nullable: true),
                    updated_on = table.Column<DateTime>(nullable: true),
                    latitude = table.Column<double>(nullable: false),
                    logo_image_id = table.Column<string>(maxLength: 200, nullable: false),
                    longitude = table.Column<double>(nullable: false),
                    post_code = table.Column<string>(maxLength: 20, nullable: false),
                    province = table.Column<string>(maxLength: 20, nullable: false),
                    short_name = table.Column<string>(maxLength: 80, nullable: true),
                    street_address = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "organizations");
        }
    }
}
