using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.GoodsService.Migrations
{
    public partial class AddIconForCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "goods_categories",
                maxLength: 800,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                table: "goods_categories");
        }
    }
}
