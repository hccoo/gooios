using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.OrderService.Migrations
{
    public partial class AddFieldInvoiceRemark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "invoice_remark",
                table: "orders",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "remark",
                table: "orders",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "invoice_remark",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "remark",
                table: "orders");
        }
    }
}
