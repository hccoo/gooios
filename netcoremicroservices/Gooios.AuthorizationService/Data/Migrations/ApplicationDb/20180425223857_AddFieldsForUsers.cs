using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.AuthorizationService.Data.Migrations.ApplicationDb
{
    public partial class AddFieldsForUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Discriminator",
            //    table: "users");

            migrationBuilder.AddColumn<string>(
                name: "nick_name",
                table: "users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "portrait_url",
                table: "users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nick_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "portrait_url",
                table: "users");

            //migrationBuilder.AddColumn<string>(
            //    name: "Discriminator",
            //    table: "users",
            //    nullable: false,
            //    defaultValue: "");
        }
    }
}
