using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.GoodsService.Migrations
{
    public partial class AddGoodsCategoryNameAndVideoUrlForGoods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoodsCategoryName",
                table: "online_goods",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoodsCategoryName",
                table: "goods",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoodsCategoryName",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "GoodsCategoryName",
                table: "goods");
        }
    }
}
