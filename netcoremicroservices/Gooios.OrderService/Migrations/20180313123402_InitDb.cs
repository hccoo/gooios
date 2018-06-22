using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.OrderService.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "delivery_notes",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    carrier_name = table.Column<string>(maxLength: 80, nullable: false),
                    carrier_phone = table.Column<string>(maxLength: 80, nullable: false),
                    consignee = table.Column<string>(maxLength: 80, nullable: false),
                    consignee_mobile = table.Column<string>(maxLength: 20, nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    delivery_address = table.Column<string>(maxLength: 80, nullable: false),
                    delivery_note_no = table.Column<string>(maxLength: 80, nullable: false),
                    order_id = table.Column<string>(maxLength: 80, nullable: false),
                    shipping_amount = table.Column<decimal>(nullable: false),
                    shipping_method = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_notes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    count = table.Column<int>(nullable: false),
                    object_id = table.Column<string>(maxLength: 80, nullable: false),
                    object_no = table.Column<string>(maxLength: 80, nullable: false),
                    order_id = table.Column<string>(maxLength: 80, nullable: false),
                    preview_picture_url = table.Column<string>(maxLength: 200, nullable: false),
                    selected_properties = table.Column<string>(maxLength: 2000, nullable: false),
                    title = table.Column<string>(maxLength: 200, nullable: false),
                    trade_unit_price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_traces",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    is_success = table.Column<bool>(nullable: false),
                    order_id = table.Column<string>(maxLength: 80, nullable: false),
                    status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_traces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    area = table.Column<string>(maxLength: 80, nullable: false),
                    city = table.Column<string>(maxLength: 80, nullable: false),
                    created_by = table.Column<string>(maxLength: 80, nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    customer_mobile = table.Column<string>(maxLength: 80, nullable: false),
                    customer_name = table.Column<string>(maxLength: 80, nullable: false),
                    invoice_type = table.Column<int>(nullable: false),
                    order_no = table.Column<string>(maxLength: 80, nullable: false),
                    pay_amount = table.Column<decimal>(nullable: false),
                    post_code = table.Column<string>(maxLength: 80, nullable: false),
                    preferential_amount = table.Column<decimal>(nullable: false),
                    province = table.Column<string>(maxLength: 80, nullable: false),
                    shipping_cost = table.Column<decimal>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    street_address = table.Column<string>(maxLength: 200, nullable: false),
                    tax = table.Column<decimal>(nullable: false),
                    total_amount = table.Column<decimal>(nullable: false),
                    updated_by = table.Column<string>(maxLength: 80, nullable: false),
                    updated_on = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "delivery_notes");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "order_traces");

            migrationBuilder.DropTable(
                name: "orders");
        }
    }
}
