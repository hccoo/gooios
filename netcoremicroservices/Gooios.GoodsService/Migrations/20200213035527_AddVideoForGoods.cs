using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.GoodsService.Migrations
{
    public partial class AddVideoForGoods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "video_path",
                table: "online_goods",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "video_path",
                table: "goods",
                maxLength: 4000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "video_path",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "video_path",
                table: "goods");
        }
    }
}
