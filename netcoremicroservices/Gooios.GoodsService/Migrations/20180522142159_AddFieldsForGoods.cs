using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.GoodsService.Migrations
{
    public partial class AddFieldsForGoods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationId",
                table: "online_goods",
                nullable: true,
                maxLength: 80,
                defaultValue: "GOOIOS001");

            migrationBuilder.AddColumn<string>(
                name: "area",
                table: "online_goods",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "online_goods",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "latitude",
                table: "online_goods",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "longitude",
                table: "online_goods",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "post_code",
                table: "online_goods",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "province",
                table: "online_goods",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "street_address",
                table: "online_goods",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationId",
                table: "goods",
                nullable: true,
                maxLength: 80,
                defaultValue: "GOOIOS001");

            migrationBuilder.AddColumn<string>(
                name: "area",
                table: "goods",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "goods",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "latitude",
                table: "goods",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "longitude",
                table: "goods",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "post_code",
                table: "goods",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "province",
                table: "goods",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "street_address",
                table: "goods",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "area",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "city",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "post_code",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "province",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "street_address",
                table: "online_goods");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "goods");

            migrationBuilder.DropColumn(
                name: "area",
                table: "goods");

            migrationBuilder.DropColumn(
                name: "city",
                table: "goods");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "goods");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "goods");

            migrationBuilder.DropColumn(
                name: "post_code",
                table: "goods");

            migrationBuilder.DropColumn(
                name: "province",
                table: "goods");

            migrationBuilder.DropColumn(
                name: "street_address",
                table: "goods");
        }
    }
}
