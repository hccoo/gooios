using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.FancyService.Migrations
{
    public partial class AddSincerityGoldField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "introduction",
                table: "services",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "street_address",
                table: "servicers",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "personal_introduction",
                table: "servicers",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 80);

            migrationBuilder.AddColumn<decimal>(
                name: "SincerityGold",
                table: "servicers",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SincerityGold",
                table: "servicers");

            migrationBuilder.AlterColumn<string>(
                name: "introduction",
                table: "services",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "street_address",
                table: "servicers",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "personal_introduction",
                table: "servicers",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
