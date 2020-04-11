using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.FancyService.Migrations
{
    public partial class AddIOSVideoUrlForService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GoodsCategoryName",
                table: "services",
                newName: "goods_category_name");

            migrationBuilder.AlterColumn<string>(
                name: "goods_category_name",
                table: "services",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ios_video_url",
                table: "services",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ios_video_url",
                table: "services");

            migrationBuilder.RenameColumn(
                name: "goods_category_name",
                table: "services",
                newName: "GoodsCategoryName");

            migrationBuilder.AlterColumn<string>(
                name: "GoodsCategoryName",
                table: "services",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
