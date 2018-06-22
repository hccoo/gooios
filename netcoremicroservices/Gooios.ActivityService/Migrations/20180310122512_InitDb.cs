using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.ActivityService.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "groupon_activities",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    count = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    end = table.Column<DateTime>(nullable: false),
                    goods_id = table.Column<string>(maxLength: 80, nullable: false),
                    updated_on = table.Column<DateTime>(nullable: false),
                    start = table.Column<DateTime>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    unit_price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groupon_activities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "groupon_participations",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    buy_count = table.Column<int>(nullable: false),
                    groupon_activity_id = table.Column<string>(maxLength: 80, nullable: false),
                    order_id = table.Column<string>(maxLength: 80, nullable: false),
                    participate_time = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groupon_participations", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "groupon_activities");

            migrationBuilder.DropTable(
                name: "groupon_participations");
        }
    }
}
