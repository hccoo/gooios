using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gooios.AuthorizationService.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;

namespace Gooios.AuthorizationService.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            
            builder.ApplyConfiguration(new AspNetUserConfiguration());
            builder.ApplyConfiguration(new AspNetUserTokenConfiguration());
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserLoginConfiguration());
            builder.ApplyConfiguration(new IdentityUserConfiguration());
            builder.ApplyConfiguration(new IdentityRoleConfiguration());
            builder.ApplyConfiguration(new IdentityRoleClaimConfiguration());
        }
    }

    public class AspNetUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("users");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.AccessFailedCount).HasColumnName("access_failed_count");
            builder.Property(c => c.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            builder.Property(c => c.Email).HasColumnName("email");
            builder.Property(c => c.EmailConfirmed).HasColumnName("email_confirmed");
            builder.Property(c => c.LockoutEnabled).HasColumnName("lockout_enabled");
            builder.Property(c => c.LockoutEnd).HasColumnName("lockout_end");
            builder.Property(c => c.NormalizedEmail).HasColumnName("normalized_email");
            builder.Property(c => c.NormalizedUserName).HasColumnName("normalized_user_name");
            builder.Property(c => c.PasswordHash).HasColumnName("password_hash");
            builder.Property(c => c.PhoneNumber).HasColumnName("phone_number");
            builder.Property(c => c.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
            builder.Property(c => c.SecurityStamp).HasColumnName("security_stamp");
            builder.Property(c => c.TwoFactorEnabled).HasColumnName("two_factor_enabled");
            builder.Property(c => c.UserName).HasColumnName("user_name");
            builder.Property(c => c.PortraitUrl).HasColumnName("portrait_url");
            builder.Property(c => c.NickName).HasColumnName("nick_name");
        }
    }
    public class AspNetUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
        {
            builder.ToTable("user_tokens");
            builder.HasKey(c => new { c.UserId, c.LoginProvider, c.Name });

            builder.Property(c => c.UserId).HasColumnName("user_id");
            builder.Property(c => c.LoginProvider).HasColumnName("login_provider");
            builder.Property(c => c.Name).HasColumnName("name");
            builder.Property(c => c.Value).HasColumnName("value");
        }
    }
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.ToTable("user_roles");
            builder.HasKey(c => new { c.RoleId, c.UserId });

            builder.Property(c => c.RoleId).HasColumnName("role_id");
            builder.Property(c => c.UserId).HasColumnName("user_id");
        }
    }
    public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
        {
            builder.ToTable("user_logins");
            builder.HasKey(c => new { c.LoginProvider, c.ProviderKey });

            builder.Property(c => c.LoginProvider).HasColumnName("login_provider");
            builder.Property(c => c.UserId).HasColumnName("user_id");
            builder.Property(c => c.ProviderDisplayName).HasColumnName("provider_display_name");
            builder.Property(c => c.ProviderKey).HasColumnName("provider_key");
        }
    }
    public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
        {
            builder.ToTable("user_claims");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ClaimType).HasColumnName("claim_type");
            builder.Property(c => c.ClaimValue).HasColumnName("claim_value");
            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.UserId).HasColumnName("user_id");
        }
    }
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<string>> builder)
        {
            builder.ToTable("roles");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Name).HasColumnName("name");
            builder.Property(c => c.NormalizedName).HasColumnName("normalized_name");
        }
    }
    public class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            builder.ToTable("role_claims");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.ClaimType).HasColumnName("claim_type");
            builder.Property(c => c.ClaimValue).HasColumnName("claim_value");
            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.RoleId).HasColumnName("role_id");
        }
    }

}
