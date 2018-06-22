using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.GoodsService.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "goods",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(maxLength: 4000, nullable: true),
                    detail = table.Column<string>(nullable: true),
                    item_number = table.Column<string>(maxLength: 80, nullable: false),
                    updated_by = table.Column<string>(maxLength: 80, nullable: false),
                    updated_on = table.Column<DateTime>(nullable: false),
                    market_price = table.Column<decimal>(nullable: false),
                    optional_property_json_object = table.Column<string>(nullable: true),
                    status = table.Column<int>(nullable: false),
                    stock = table.Column<int>(nullable: false),
                    store_id = table.Column<string>(maxLength: 80, nullable: false),
                    SubCategory = table.Column<string>(nullable: true),
                    title = table.Column<string>(maxLength: 200, nullable: false),
                    unit = table.Column<string>(nullable: false),
                    unit_price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "goods_categories",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    name = table.Column<string>(maxLength: 80, nullable: false),
                    parent_id = table.Column<string>(maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goods_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "goods_images",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_on = table.Column<DateTime>(nullable: false),
                    goods_id = table.Column<string>(maxLength: 80, nullable: false),
                    image_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goods_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "groupon_conditions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    goods_id = table.Column<string>(maxLength: 80, nullable: false),
                    more_than_number = table.Column<int>(nullable: false),
                    price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groupon_conditions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "online_goods",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(maxLength: 4000, nullable: true),
                    detail = table.Column<string>(nullable: true),
                    item_number = table.Column<string>(maxLength: 80, nullable: false),
                    updated_by = table.Column<string>(maxLength: 80, nullable: false),
                    updated_on = table.Column<DateTime>(nullable: false),
                    market_price = table.Column<decimal>(nullable: false),
                    optional_property_json_object = table.Column<string>(nullable: true),
                    status = table.Column<int>(nullable: false),
                    stock = table.Column<int>(nullable: false),
                    store_id = table.Column<string>(maxLength: 80, nullable: false),
                    SubCategory = table.Column<string>(nullable: true),
                    title = table.Column<string>(maxLength: 200, nullable: false),
                    unit = table.Column<string>(nullable: false),
                    unit_price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_online_goods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "online_goods_images",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_on = table.Column<DateTime>(nullable: false),
                    goods_id = table.Column<string>(maxLength: 80, nullable: false),
                    image_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_online_goods_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "online_groupon_conditions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    goods_id = table.Column<string>(maxLength: 80, nullable: false),
                    more_than_number = table.Column<int>(nullable: false),
                    price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_online_groupon_conditions", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "goods");

            migrationBuilder.DropTable(
                name: "goods_categories");

            migrationBuilder.DropTable(
                name: "goods_images");

            migrationBuilder.DropTable(
                name: "groupon_conditions");

            migrationBuilder.DropTable(
                name: "online_goods");

            migrationBuilder.DropTable(
                name: "online_goods_images");

            migrationBuilder.DropTable(
                name: "online_groupon_conditions");
        }
    }
}
