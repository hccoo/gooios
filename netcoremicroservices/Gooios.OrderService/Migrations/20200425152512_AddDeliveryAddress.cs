using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.OrderService.Migrations
{
    public partial class AddDeliveryAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "delivery_address_id",
                table: "orders",
                maxLength: 80,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "delivery_addresses",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    area = table.Column<string>(maxLength: 80, nullable: false),
                    City = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    gender = table.Column<int>(nullable: false),
                    is_default = table.Column<bool>(nullable: false),
                    link_man = table.Column<string>(maxLength: 80, nullable: false),
                    mark = table.Column<string>(maxLength: 80, nullable: true),
                    mobile = table.Column<string>(maxLength: 80, nullable: false),
                    postcode = table.Column<string>(maxLength: 80, nullable: true),
                    province = table.Column<string>(nullable: false),
                    street_address = table.Column<string>(nullable: false),
                    user_id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_addresses", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "delivery_addresses");

            migrationBuilder.DropColumn(
                name: "delivery_address_id",
                table: "orders");
        }
    }
}
