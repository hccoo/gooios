using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.GoodsService.Migrations
{
    public partial class AddOrderForOnlineGoods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "online_goods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "recommend_level",
                table: "online_goods",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "recommend_level",
                table: "online_goods");
        }
    }
}
