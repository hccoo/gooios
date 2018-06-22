using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.ActivityService.Migrations
{
    public partial class AddFieldsForTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "creator_name",
                table: "topics",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "creator_portrait_url",
                table: "topics",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "creator_name",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "creator_portrait_url",
                table: "topics");
        }
    }
}
