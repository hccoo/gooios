using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.FancyService.Migrations
{
    public partial class AddApplicationIdForTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "application_id",
                table: "services",
                maxLength: 80,
                nullable: false,
                defaultValue: "GOOIOS001");

            migrationBuilder.AddColumn<string>(
                name: "application_id",
                table: "servicers",
                maxLength: 80,
                nullable: false,
                defaultValue: "GOOIOS001");

            migrationBuilder.AddColumn<string>(
                name: "application_id",
                table: "categories",
                maxLength: 80,
                nullable: false,
                defaultValue: "GOOIOS001");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "application_id",
                table: "services");

            migrationBuilder.DropColumn(
                name: "application_id",
                table: "servicers");

            migrationBuilder.DropColumn(
                name: "application_id",
                table: "categories");
        }
    }
}
