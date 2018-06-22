using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Gooios.AuthorizationService.Data.Migrations.ConfigurationCustomDb
{
    public partial class InitConfigurationCustomDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "api_resources",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(nullable: true),
                    display_name = table.Column<string>(nullable: true),
                    enabled = table.Column<bool>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_api_resources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    absolute_refresh_token_lifetime = table.Column<int>(nullable: false),
                    access_token_lifetime = table.Column<int>(nullable: false),
                    access_token_type = table.Column<int>(nullable: false),
                    allow_access_tokens_via_browser = table.Column<bool>(nullable: false),
                    allow_offline_access = table.Column<bool>(nullable: false),
                    allow_plain_text_pkce = table.Column<bool>(nullable: false),
                    allow_remember_consent = table.Column<bool>(nullable: false),
                    always_include_user_claims_in_id_token = table.Column<bool>(nullable: false),
                    always_send_client_claims = table.Column<bool>(nullable: false),
                    authorization_code_lifetime = table.Column<int>(nullable: false),
                    back_channel_logout_session_required = table.Column<bool>(nullable: false),
                    back_channel_logout_uri = table.Column<string>(nullable: true),
                    client_claims_prefix = table.Column<string>(nullable: true),
                    client_id = table.Column<string>(nullable: true),
                    client_name = table.Column<string>(nullable: true),
                    client_uri = table.Column<string>(nullable: true),
                    consent_lifetime = table.Column<int>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    enable_local_login = table.Column<bool>(nullable: false),
                    enabled = table.Column<bool>(nullable: false),
                    front_channel_logout_session_required = table.Column<bool>(nullable: false),
                    front_channel_logout_uri = table.Column<string>(nullable: true),
                    identity_token_lifetime = table.Column<int>(nullable: false),
                    include_jwt_id = table.Column<bool>(nullable: false),
                    logo_uri = table.Column<string>(nullable: true),
                    pair_wise_subject_salt = table.Column<string>(nullable: true),
                    protocol_type = table.Column<string>(nullable: true),
                    refresh_token_expiration = table.Column<int>(nullable: false),
                    refresh_token_usage = table.Column<int>(nullable: false),
                    require_client_secret = table.Column<bool>(nullable: false),
                    require_consent = table.Column<bool>(nullable: false),
                    require_pkce = table.Column<bool>(nullable: false),
                    sliding_refresh_token_lifetime = table.Column<int>(nullable: false),
                    update_access_token_claims_on_refresh = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identity_resources",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(nullable: true),
                    display_name = table.Column<string>(nullable: true),
                    emphasize = table.Column<bool>(nullable: false),
                    enabled = table.Column<bool>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    required = table.Column<bool>(nullable: false),
                    show_in_discovery_document = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_resources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "api_resource_claims",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    api_resource_id = table.Column<int>(nullable: false),
                    type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_api_resource_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_api_resource_claims_api_resources_api_resource_id",
                        column: x => x.api_resource_id,
                        principalTable: "api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "api_scopes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    api_resource_id = table.Column<int>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    display_name = table.Column<string>(nullable: true),
                    emphasize = table.Column<bool>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    required = table.Column<bool>(nullable: false),
                    show_in_discovery_document = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_api_scopes", x => x.id);
                    table.ForeignKey(
                        name: "fk_api_scopes_api_resources_api_resource_id",
                        column: x => x.api_resource_id,
                        principalTable: "api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "api_secrets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    api_resource_id = table.Column<int>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    expiration = table.Column<DateTime>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_api_secrets", x => x.id);
                    table.ForeignKey(
                        name: "fk_api_secrets_api_resources_api_resource_id",
                        column: x => x.api_resource_id,
                        principalTable: "api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_claims",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_claims_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_cors_origins",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(nullable: false),
                    origin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_cors_origins", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_cors_origins_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_grant_types",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(nullable: false),
                    GrantType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_grant_types", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_grant_types_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_id_prestrictions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(nullable: false),
                    provider = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_id_prestrictions", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_id_prestrictions_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_post_logout_redirect_uris",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(nullable: false),
                    post_logout_redirect_uri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_post_logout_redirect_uris", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_post_logout_redirect_uris_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_properties",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(nullable: false),
                    key = table.Column<string>(nullable: true),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_properties", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_properties_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_redirect_uris",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(nullable: false),
                    redirect_uri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_redirect_uris", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_redirect_uris_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_scopes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(nullable: false),
                    scope = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_scopes", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_scopes_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_secrets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    expiration = table.Column<DateTime>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_secrets", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_secrets_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_claims",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    identity_resource_id = table.Column<int>(nullable: false),
                    type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_identity_claims_identity_resources_identity_resource_id",
                        column: x => x.identity_resource_id,
                        principalTable: "identity_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "api_scope_claims",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    api_scope_id = table.Column<int>(nullable: false),
                    type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_api_scope_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_api_scope_claims_api_scopes_api_scope_id",
                        column: x => x.api_scope_id,
                        principalTable: "api_scopes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_api_resource_claims_api_resource_id",
                table: "api_resource_claims",
                column: "api_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_api_scope_claims_api_scope_id",
                table: "api_scope_claims",
                column: "api_scope_id");

            migrationBuilder.CreateIndex(
                name: "ix_api_scopes_api_resource_id",
                table: "api_scopes",
                column: "api_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_api_secrets_api_resource_id",
                table: "api_secrets",
                column: "api_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_claims_client_id",
                table: "client_claims",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_cors_origins_client_id",
                table: "client_cors_origins",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_grant_types_client_id",
                table: "client_grant_types",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_id_prestrictions_client_id",
                table: "client_id_prestrictions",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_post_logout_redirect_uris_client_id",
                table: "client_post_logout_redirect_uris",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_properties_client_id",
                table: "client_properties",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_redirect_uris_client_id",
                table: "client_redirect_uris",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_scopes_client_id",
                table: "client_scopes",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_secrets_client_id",
                table: "client_secrets",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_identity_claims_identity_resource_id",
                table: "identity_claims",
                column: "identity_resource_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "api_resource_claims");

            migrationBuilder.DropTable(
                name: "api_scope_claims");

            migrationBuilder.DropTable(
                name: "api_secrets");

            migrationBuilder.DropTable(
                name: "client_claims");

            migrationBuilder.DropTable(
                name: "client_cors_origins");

            migrationBuilder.DropTable(
                name: "client_grant_types");

            migrationBuilder.DropTable(
                name: "client_id_prestrictions");

            migrationBuilder.DropTable(
                name: "client_post_logout_redirect_uris");

            migrationBuilder.DropTable(
                name: "client_properties");

            migrationBuilder.DropTable(
                name: "client_redirect_uris");

            migrationBuilder.DropTable(
                name: "client_scopes");

            migrationBuilder.DropTable(
                name: "client_secrets");

            migrationBuilder.DropTable(
                name: "identity_claims");

            migrationBuilder.DropTable(
                name: "api_scopes");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "identity_resources");

            migrationBuilder.DropTable(
                name: "api_resources");
        }
    }
}
