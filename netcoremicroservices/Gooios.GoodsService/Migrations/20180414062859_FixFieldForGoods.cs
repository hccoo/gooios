using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.GoodsService.Migrations
{
    public partial class FixFieldForGoods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubCategory",
                table: "goods",
                newName: "sub_category");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "goods",
                newName: "category");

            migrationBuilder.AlterColumn<string>(
                name: "sub_category",
                table: "goods",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "category",
                table: "goods",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sub_category",
                table: "goods",
                newName: "SubCategory");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "goods",
                newName: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "SubCategory",
                table: "goods",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "goods",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 80);
        }
    }
}
