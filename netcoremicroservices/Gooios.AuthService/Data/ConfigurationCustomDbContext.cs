using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Gooios.AuthService.Data
{
    public class ConfigurationCustomDbContext : DbContext, IConfigurationDbContext
    {
        public ConfigurationCustomDbContext(DbContextOptions<ConfigurationCustomDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<ApiSecret> ApiSecrets { get; set; }
        public DbSet<IdentityClaim> IdentityClaims { get; set; }
        public DbSet<ClientClaim> ClientClaims { get; set; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public DbSet<ClientGrantType> ClientGrantTypes { get; set; }
        public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }
        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        public DbSet<ClientProperty> ClientProperties { get; set; }
        public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        public DbSet<ClientScope> ClientScopes { get; set; }
        public DbSet<ClientSecret> ClientSecrets { get; set; }
        public DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new ClientConfiguration());
            //modelBuilder.ApplyConfiguration(new ApiResourceConfiguration());
            //modelBuilder.ApplyConfiguration(new IdentityResourceConfiguration());
            //modelBuilder.ApplyConfiguration(new ApiResourceClaimConfiguration());
            //modelBuilder.ApplyConfiguration(new ApiScopeConfiguration());
            //modelBuilder.ApplyConfiguration(new ApiSecretConfiguration());
            //modelBuilder.ApplyConfiguration(new IdentityClaimConfiguration());
            //modelBuilder.ApplyConfiguration(new ClientClaimConfiguration());
            //modelBuilder.ApplyConfiguration(new ClientCorsOriginConfiguration());
            //modelBuilder.ApplyConfiguration(new ClientGrantTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new ClientIdPRestrictionConfiguration());
            //modelBuilder.ApplyConfiguration(new ClientPostLogoutRedirectUriConfiguration());
            //modelBuilder.ApplyConfiguration(new ClientPropertyConfiguration());
            //modelBuilder.ApplyConfiguration(new ClientRedirectUriConfiguration());
            //modelBuilder.ApplyConfiguration(new ClientScopeConfiguration());
            //modelBuilder.ApplyConfiguration(new ClientSecretConfiguration());
            //modelBuilder.ApplyConfiguration(new ApiScopeClaimConfiguration());

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql("Database='gooios_authservice_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true");
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }

    public class ConfigurationCustomDbContextFactory : IDesignTimeDbContextFactory<ConfigurationCustomDbContext>
    {
        public ConfigurationCustomDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ConfigurationCustomDbContext>() { };
            return new ConfigurationCustomDbContext(builder.Options);
        }
    }

    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public ClientConfiguration() { }

        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable<Client>("clients");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ClientId).HasColumnName("client_id");
            builder.Property(c => c.AbsoluteRefreshTokenLifetime).HasColumnName("absolute_refresh_token_lifetime");
            builder.Property(c => c.AccessTokenLifetime).HasColumnName("access_token_lifetime");
            builder.Property(c => c.AccessTokenType).HasColumnName("access_token_type");
            builder.Property(c => c.AllowAccessTokensViaBrowser).HasColumnName("allow_access_tokens_via_browser");
            builder.Property(c => c.AllowOfflineAccess).HasColumnName("allow_offline_access");
            builder.Property(c => c.AllowPlainTextPkce).HasColumnName("allow_plain_text_pkce");
            builder.Property(c => c.AllowRememberConsent).HasColumnName("allow_remember_consent");
            builder.Property(c => c.AlwaysIncludeUserClaimsInIdToken).HasColumnName("always_include_user_claims_in_id_token");
            builder.Property(c => c.AlwaysSendClientClaims).HasColumnName("always_send_client_claims");
            builder.Property(c => c.AuthorizationCodeLifetime).HasColumnName("authorization_code_lifetime");
            builder.Property(c => c.BackChannelLogoutSessionRequired).HasColumnName("back_channel_logout_session_required");
            builder.Property(c => c.BackChannelLogoutUri).HasColumnName("back_channel_logout_uri");
            builder.Property(c => c.ClientClaimsPrefix).HasColumnName("client_claims_prefix");
            builder.Property(c => c.ClientId).HasColumnName("client_id");
            builder.Property(c => c.ClientName).HasColumnName("client_name");
            builder.Property(c => c.ClientUri).HasColumnName("client_uri");
            builder.Property(c => c.ConsentLifetime).HasColumnName("consent_lifetime");
            builder.Property(c => c.Description).HasColumnName("description");
            builder.Property(c => c.Enabled).HasColumnName("enabled");
            builder.Property(c => c.EnableLocalLogin).HasColumnName("enable_local_login");
            builder.Property(c => c.FrontChannelLogoutSessionRequired).HasColumnName("front_channel_logout_session_required");
            builder.Property(c => c.FrontChannelLogoutUri).HasColumnName("front_channel_logout_uri");
            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.IdentityTokenLifetime).HasColumnName("identity_token_lifetime");
            builder.Property(c => c.IncludeJwtId).HasColumnName("include_jwt_id");
            builder.Property(c => c.LogoUri).HasColumnName("logo_uri");
            builder.Property(c => c.PairWiseSubjectSalt).HasColumnName("pair_wise_subject_salt");
            builder.Property(c => c.ProtocolType).HasColumnName("protocol_type");
            builder.Property(c => c.RefreshTokenExpiration).HasColumnName("refresh_token_expiration");
            builder.Property(c => c.RefreshTokenUsage).HasColumnName("refresh_token_usage");
            builder.Property(c => c.RequireClientSecret).HasColumnName("require_client_secret");
            builder.Property(c => c.RequireConsent).HasColumnName("require_consent");
            builder.Property(c => c.RequirePkce).HasColumnName("require_pkce");
            builder.Property(c => c.SlidingRefreshTokenLifetime).HasColumnName("sliding_refresh_token_lifetime");
            builder.Property(c => c.UpdateAccessTokenClaimsOnRefresh).HasColumnName("update_access_token_claims_on_refresh");
        }
    }

    public class ApiResourceConfiguration : IEntityTypeConfiguration<ApiResource>
    {
        public void Configure(EntityTypeBuilder<ApiResource> builder)
        {
            builder.ToTable<ApiResource>("api_resources");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Description).HasColumnName("description");
            builder.Property(c => c.DisplayName).HasColumnName("display_name");
            builder.Property(c => c.Enabled).HasColumnName("enabled");
            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Name).HasColumnName("name");
        }
    }

    public class IdentityResourceConfiguration : IEntityTypeConfiguration<IdentityResource>
    {
        public void Configure(EntityTypeBuilder<IdentityResource> builder)
        {
            builder.ToTable<IdentityResource>("identity_resources");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Description).HasColumnName("description");
            builder.Property(c => c.DisplayName).HasColumnName("display_name");
            builder.Property(c => c.Emphasize).HasColumnName("emphasize");
            builder.Property(c => c.Enabled).HasColumnName("enabled");
            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Name).HasColumnName("name");
            builder.Property(c => c.Required).HasColumnName("required");
            builder.Property(c => c.ShowInDiscoveryDocument).HasColumnName("show_in_discovery_document");
        }
    }

    public class ApiResourceClaimConfiguration : IEntityTypeConfiguration<ApiResourceClaim>
    {
        public void Configure(EntityTypeBuilder<ApiResourceClaim> builder)
        {
            builder.ToTable<ApiResourceClaim>("api_resource_claims");
            builder.HasKey(c => c.Id);

            builder.Property<int>("ApiResourceId").HasColumnName("api_resource_id");
            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Type).HasColumnName("type");

            builder.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource")
                .WithMany("UserClaims")
                .HasForeignKey("ApiResourceId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ApiScopeConfiguration : IEntityTypeConfiguration<ApiScope>
    {
        public void Configure(EntityTypeBuilder<ApiScope> builder)
        {
            builder.ToTable<ApiScope>("api_scopes");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Description).HasColumnName("description");
            builder.Property(c => c.DisplayName).HasColumnName("display_name");
            builder.Property(c => c.Emphasize).HasColumnName("emphasize");
            builder.Property(c => c.Name).HasColumnName("name");
            builder.Property(c => c.Required).HasColumnName("required");
            builder.Property(c => c.ShowInDiscoveryDocument).HasColumnName("show_in_discovery_document");

            builder.Property<int>("ApiResourceId").HasColumnName("api_resource_id");

            builder.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource")
                .WithMany("Scopes")
                .HasForeignKey("ApiResourceId")
                .OnDelete(DeleteBehavior.Cascade);

        }
    }

    public class ApiSecretConfiguration : IEntityTypeConfiguration<ApiSecret>
    {
        public void Configure(EntityTypeBuilder<ApiSecret> builder)
        {
            builder.ToTable<ApiSecret>("api_secrets");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Description).HasColumnName("description");
            builder.Property(c => c.Expiration).HasColumnName("expiration");
            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Type).HasColumnName("type");
            builder.Property(c => c.Value).HasColumnName("value");

            builder.Property<int>("ApiResourceId").HasColumnName("api_resource_id");

            builder.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource")
                .WithMany("Secrets")
                .HasForeignKey("ApiResourceId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class IdentityClaimConfiguration : IEntityTypeConfiguration<IdentityClaim>
    {
        public void Configure(EntityTypeBuilder<IdentityClaim> builder)
        {
            builder.ToTable<IdentityClaim>("identity_claims");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Type).HasColumnName("type");

            builder.Property<int>("IdentityResourceId").HasColumnName("identity_resource_id");
            builder.HasOne("IdentityServer4.EntityFramework.Entities.IdentityResource", "IdentityResource")
                .WithMany("UserClaims")
                .HasForeignKey("IdentityResourceId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClientClaimConfiguration : IEntityTypeConfiguration<ClientClaim>
    {
        public void Configure(EntityTypeBuilder<ClientClaim> builder)
        {
            builder.ToTable<ClientClaim>("client_claims");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Type).HasColumnName("type");
            builder.Property(c => c.Value).HasColumnName("value");

            builder.Property<int>("ClientId").HasColumnName("client_id");
            builder.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                .WithMany("Claims")
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClientCorsOriginConfiguration : IEntityTypeConfiguration<ClientCorsOrigin>
    {
        public void Configure(EntityTypeBuilder<ClientCorsOrigin> builder)
        {
            builder.ToTable<ClientCorsOrigin>("client_cors_origins");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Origin).HasColumnName("origin");

            builder.Property<int>("ClientId").HasColumnName("client_id");
            builder.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                .WithMany("AllowedCorsOrigins")
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClientGrantTypeConfiguration : IEntityTypeConfiguration<ClientGrantType>
    {
        public void Configure(EntityTypeBuilder<ClientGrantType> builder)
        {
            builder.ToTable<ClientGrantType>("client_grant_types");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("id");

            builder.Property<int>("ClientId").HasColumnName("client_id");
            builder.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                .WithMany("AllowedGrantTypes")
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClientIdPRestrictionConfiguration : IEntityTypeConfiguration<ClientIdPRestriction>
    {
        public void Configure(EntityTypeBuilder<ClientIdPRestriction> builder)
        {
            builder.ToTable<ClientIdPRestriction>("client_id_prestrictions");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Provider).HasColumnName("provider");

            builder.Property<int>("ClientId").HasColumnName("client_id");
            builder.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                .WithMany("IdentityProviderRestrictions")
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClientPostLogoutRedirectUriConfiguration : IEntityTypeConfiguration<ClientPostLogoutRedirectUri>
    {
        public void Configure(EntityTypeBuilder<ClientPostLogoutRedirectUri> builder)
        {
            builder.ToTable<ClientPostLogoutRedirectUri>("client_post_logout_redirect_uris");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.PostLogoutRedirectUri).HasColumnName("post_logout_redirect_uri");

            builder.Property<int>("ClientId").HasColumnName("client_id");
            builder.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                .WithMany("PostLogoutRedirectUris")
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClientPropertyConfiguration : IEntityTypeConfiguration<ClientProperty>
    {
        public void Configure(EntityTypeBuilder<ClientProperty> builder)
        {
            builder.ToTable<ClientProperty>("client_properties");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Key).HasColumnName("key");
            builder.Property(c => c.Value).HasColumnName("value");

            builder.Property<int>("ClientId").HasColumnName("client_id");
            builder.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                .WithMany("Properties")
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClientRedirectUriConfiguration : IEntityTypeConfiguration<ClientRedirectUri>
    {
        public void Configure(EntityTypeBuilder<ClientRedirectUri> builder)
        {
            builder.ToTable<ClientRedirectUri>("client_redirect_uris");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.RedirectUri).HasColumnName("redirect_uri");

            builder.Property<int>("ClientId").HasColumnName("client_id");
            builder.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                .WithMany("RedirectUris")
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClientScopeConfiguration : IEntityTypeConfiguration<ClientScope>
    {
        public void Configure(EntityTypeBuilder<ClientScope> builder)
        {
            builder.ToTable<ClientScope>("client_scopes");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Scope).HasColumnName("scope");

            builder.Property<int>("ClientId").HasColumnName("client_id");
            builder.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                .WithMany("AllowedScopes")
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClientSecretConfiguration : IEntityTypeConfiguration<ClientSecret>
    {
        public void Configure(EntityTypeBuilder<ClientSecret> builder)
        {
            builder.ToTable<ClientSecret>("client_secrets");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Description).HasColumnName("description");
            builder.Property(c => c.Expiration).HasColumnName("expiration");
            builder.Property(c => c.Value).HasColumnName("value");
            builder.Property(c => c.Type).HasColumnName("type");

            builder.Property<int>("ClientId").HasColumnName("client_id");
            builder.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                .WithMany("ClientSecrets")
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ApiScopeClaimConfiguration : IEntityTypeConfiguration<ApiScopeClaim>
    {
        public void Configure(EntityTypeBuilder<ApiScopeClaim> builder)
        {
            builder.ToTable<ApiScopeClaim>("api_scope_claims");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Type).HasColumnName("type");

            builder.Property<int>("ApiScopeId").HasColumnName("api_scope_id");
            builder.HasOne("IdentityServer4.EntityFramework.Entities.ApiScope", "ApiScope")
                .WithMany("UserClaims")
                .HasForeignKey("ApiScopeId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }


}
