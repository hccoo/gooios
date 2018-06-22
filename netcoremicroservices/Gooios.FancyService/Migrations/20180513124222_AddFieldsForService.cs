using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.FancyService.Migrations
{
    public partial class AddFieldsForService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_advertisement",
                table: "services",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "personalized_page_uri",
                table: "services",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_advertisement",
                table: "services");

            migrationBuilder.DropColumn(
                name: "personalized_page_uri",
                table: "services");
        }
    }
}
