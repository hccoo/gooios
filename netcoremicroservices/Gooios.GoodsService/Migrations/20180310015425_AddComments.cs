using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.GoodsService.Migrations
{
    public partial class AddComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    goods_id = table.Column<string>(maxLength: 80, nullable: false),
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
                    content = table.Column<string>(maxLength: 80, nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    goods_id = table.Column<string>(maxLength: 80, nullable: false),
                    order_id = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.id);
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
                name: "comment_images");

            migrationBuilder.DropTable(
                name: "comment_tags");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "tags");
        }
    }
}
