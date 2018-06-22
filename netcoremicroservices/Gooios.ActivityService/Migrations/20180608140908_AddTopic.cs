using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.ActivityService.Migrations
{
    public partial class AddTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "topic_images",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    image_id = table.Column<string>(maxLength: 80, nullable: false),
                    image_url = table.Column<string>(maxLength: 500, nullable: false),
                    topic_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    area = table.Column<string>(maxLength: 80, nullable: false),
                    category = table.Column<int>(nullable: true),
                    city = table.Column<string>(maxLength: 80, nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    custom_topic_url = table.Column<string>(maxLength: 500, nullable: true),
                    end_date = table.Column<DateTime>(nullable: true),
                    face_image_url = table.Column<string>(maxLength: 500, nullable: false),
                    introduction = table.Column<string>(nullable: false),
                    is_custom = table.Column<bool>(nullable: false),
                    is_suspend = table.Column<bool>(nullable: false),
                    updated_by = table.Column<string>(maxLength: 80, nullable: true),
                    updated_on = table.Column<DateTime>(nullable: false),
                    latitude = table.Column<double>(nullable: false),
                    longitude = table.Column<double>(nullable: false),
                    organization_id = table.Column<string>(maxLength: 80, nullable: true),
                    postcode = table.Column<string>(maxLength: 80, nullable: true),
                    province = table.Column<string>(maxLength: 80, nullable: false),
                    start_date = table.Column<DateTime>(nullable: true),
                    street_address = table.Column<string>(maxLength: 200, nullable: false),
                    title = table.Column<string>(maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "topic_images");

            migrationBuilder.DropTable(
                name: "topics");
        }
    }
}
