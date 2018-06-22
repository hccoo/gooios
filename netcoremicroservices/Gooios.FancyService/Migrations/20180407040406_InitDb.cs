using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.FancyService.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    mark = table.Column<string>(maxLength: 80, nullable: false),
                    name = table.Column<string>(maxLength: 80, nullable: false),
                    parent_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "comment_images",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    comment_id = table.Column<string>(maxLength: 80, nullable: false),
                    image_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "comment_tags",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    comment_id = table.Column<string>(maxLength: 80, nullable: false),
                    reservation_id = table.Column<string>(maxLength: 80, nullable: false),
                    tag_id = table.Column<string>(maxLength: 80, nullable: false),
                    user_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    content = table.Column<string>(maxLength: 500, nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    order_id = table.Column<string>(maxLength: 80, nullable: false),
                    reservation_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    appoint_time = table.Column<DateTime>(nullable: false),
                    area = table.Column<string>(maxLength: 80, nullable: false),
                    city = table.Column<string>(maxLength: 80, nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    customer_mobile = table.Column<string>(maxLength: 80, nullable: false),
                    customer_name = table.Column<string>(maxLength: 80, nullable: false),
                    updated_by = table.Column<string>(maxLength: 80, nullable: true),
                    updated_on = table.Column<DateTime>(nullable: false),
                    latitude = table.Column<double>(nullable: false),
                    longitude = table.Column<double>(nullable: false),
                    postcode = table.Column<string>(maxLength: 80, nullable: false),
                    province = table.Column<string>(maxLength: 80, nullable: false),
                    reservation_no = table.Column<string>(maxLength: 80, nullable: false),
                    service_id = table.Column<string>(maxLength: 80, nullable: true),
                    servicer_id = table.Column<string>(maxLength: 80, nullable: true),
                    sincerity_gold_need_to_pay = table.Column<decimal>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    street_address = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service_images",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_on = table.Column<DateTime>(nullable: false),
                    image_id = table.Column<string>(maxLength: 80, nullable: false),
                    service_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "servicer_images",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_on = table.Column<DateTime>(nullable: false),
                    image_id = table.Column<string>(maxLength: 80, nullable: false),
                    servicer_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicer_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "servicers",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    area = table.Column<string>(maxLength: 80, nullable: false),
                    birth_day = table.Column<DateTime>(nullable: false),
                    category = table.Column<string>(maxLength: 80, nullable: false),
                    city = table.Column<string>(maxLength: 80, nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    gender = table.Column<int>(nullable: false),
                    is_suspend = table.Column<bool>(nullable: false),
                    job_number = table.Column<string>(maxLength: 80, nullable: false),
                    updated_by = table.Column<string>(maxLength: 80, nullable: true),
                    updated_at = table.Column<DateTime>(maxLength: 80, nullable: false),
                    latitude = table.Column<double>(nullable: false),
                    longitude = table.Column<double>(nullable: false),
                    name = table.Column<string>(maxLength: 80, nullable: false),
                    organization_id = table.Column<string>(maxLength: 80, nullable: false),
                    personal_introduction = table.Column<string>(maxLength: 80, nullable: false),
                    portrait_image_id = table.Column<string>(maxLength: 80, nullable: false),
                    postcode = table.Column<string>(maxLength: 80, nullable: false),
                    province = table.Column<string>(maxLength: 80, nullable: false),
                    sincerity_gold_rate = table.Column<double>(nullable: false),
                    start_relevant_work_time = table.Column<DateTime>(nullable: false),
                    street_address = table.Column<string>(maxLength: 80, nullable: false),
                    sub_category = table.Column<string>(maxLength: 80, nullable: false),
                    technical_grade = table.Column<int>(nullable: false),
                    technical_title = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    area = table.Column<string>(maxLength: 80, nullable: false),
                    category = table.Column<string>(maxLength: 80, nullable: false),
                    city = table.Column<string>(maxLength: 80, nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    introduction = table.Column<string>(maxLength: 200, nullable: false),
                    updated_by = table.Column<string>(maxLength: 80, nullable: true),
                    updated_at = table.Column<DateTime>(maxLength: 80, nullable: false),
                    latitude = table.Column<double>(nullable: false),
                    longitude = table.Column<double>(nullable: false),
                    organization_id = table.Column<string>(maxLength: 80, nullable: false),
                    postcode = table.Column<string>(maxLength: 80, nullable: false),
                    province = table.Column<string>(maxLength: 80, nullable: false),
                    serve_scope = table.Column<int>(nullable: false),
                    sincerity_gold = table.Column<decimal>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    street_address = table.Column<string>(maxLength: 80, nullable: false),
                    sub_category = table.Column<string>(maxLength: 80, nullable: false),
                    title = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    category_id = table.Column<string>(maxLength: 80, nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "comment_images");

            migrationBuilder.DropTable(
                name: "comment_tags");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "service_images");

            migrationBuilder.DropTable(
                name: "servicer_images");

            migrationBuilder.DropTable(
                name: "servicers");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "tags");
        }
    }
}
