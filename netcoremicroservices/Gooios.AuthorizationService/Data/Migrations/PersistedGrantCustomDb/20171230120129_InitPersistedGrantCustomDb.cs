using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.AuthorizationService.Data.Migrations.PersistedGrantCustomDb
{
    public partial class InitPersistedGrantCustomDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "persisted_grants",
                columns: table => new
                {
                    key = table.Column<string>(nullable: false),
                    client_id = table.Column<string>(nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    data = table.Column<string>(nullable: true),
                    expiration = table.Column<DateTime>(nullable: true),
                    subject_id = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_persisted_grants", x => x.key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "persisted_grants");
        }
    }
}
