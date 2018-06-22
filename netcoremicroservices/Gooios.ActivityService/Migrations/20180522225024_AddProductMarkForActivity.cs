using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.ActivityService.Migrations
{
    public partial class AddProductMarkForActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "goods_id",
                table: "groupon_activities",
                newName: "product_mark");

            migrationBuilder.AddColumn<string>(
                name: "product_id",
                table: "groupon_activities",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_id",
                table: "groupon_activities");

            migrationBuilder.RenameColumn(
                name: "product_mark",
                table: "groupon_activities",
                newName: "goods_id");
        }
    }
}
