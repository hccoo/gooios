using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.GoodsService.Migrations
{
    public partial class AddDistributionScopeForGoods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "distribution_scope",
                table: "online_goods",
                maxLength: 80,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "distribution_scope",
                table: "goods",
                maxLength: 80,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "distribution_scope",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "distribution_scope",
                table: "goods");
        }
    }
}
