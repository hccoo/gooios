using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.GoodsService.Migrations
{
    public partial class AddApplicationIdField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "online_goods",
                newName: "application_id");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "goods",
                newName: "application_id");

            migrationBuilder.AlterColumn<string>(
                name: "application_id",
                table: "online_goods",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "application_id",
                table: "goods_categories",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "application_id",
                table: "goods",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "application_id",
                table: "goods_categories");

            migrationBuilder.RenameColumn(
                name: "application_id",
                table: "online_goods",
                newName: "ApplicationId");

            migrationBuilder.RenameColumn(
                name: "application_id",
                table: "goods",
                newName: "ApplicationId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationId",
                table: "online_goods",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationId",
                table: "goods",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 80);
        }
    }
}
