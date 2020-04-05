using Gooios.UserService.Configurations;
using Gooios.UserService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.UserService.Repositories
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options) { }

        public DbSet<CookAppUser> CookAppUsers { get; set; }

        public DbSet<CookAppPartnerLoginUser> CookAppPartnerLoginUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CookAppUserConfiguration());
            builder.ApplyConfiguration(new CookAppPartnerLoginUserConfiguration());
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var conn = IocProvider.GetService<IServiceConfigurationAgent>()?.ConnectionString;
        //    if (!optionsBuilder.IsConfigured) optionsBuilder.UseMySql(conn);

        //    //optionsBuilder.UseMySql("Database='gooios_user_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true");
        //}
    }

    public class CookAppUserConfiguration : IEntityTypeConfiguration<CookAppUser>
    {
        public void Configure(EntityTypeBuilder<CookAppUser> builder)
        {
            builder.ToTable("cookapp_users");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.UserName).HasColumnName("user_name").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Password).HasColumnName("password").HasMaxLength(200).IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.UpdatedBy).HasColumnName("updated_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.UpdatedOn).HasColumnName("updated_on").IsRequired();
            builder.Property(c => c.Email).HasColumnName("email").HasMaxLength(200);
            builder.Property(c => c.Mobile).HasColumnName("mobile").HasMaxLength(20);
        }
    }

    public class CookAppPartnerLoginUserConfiguration : IEntityTypeConfiguration<CookAppPartnerLoginUser>
    {
        public void Configure(EntityTypeBuilder<CookAppPartnerLoginUser> builder)
        {
            builder.ToTable("cookapp_partiner_loginusers");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.ExpiredIn).HasColumnName("expired_in");
            builder.Property(c => c.LoginChannel).HasColumnName("login_channel");
            builder.Property(c => c.PartnerAccessToken).HasColumnName("partner_access_token").HasMaxLength(2000);
            builder.Property(c => c.PartnerAuthCode).HasColumnName("partner_auth_code").HasMaxLength(200);
            builder.Property(c => c.PartnerKey).HasColumnName("partner_key").HasMaxLength(200).IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.UpdatedBy).HasColumnName("updated_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.UpdatedOn).HasColumnName("updated_on").IsRequired();
            builder.Property(c => c.RefreshToken).HasColumnName("refresh_token").HasMaxLength(1000);
            builder.Property(c => c.Scope).HasColumnName("scope").HasMaxLength(200);
            builder.Property(c => c.UnionId).HasColumnName("union_id").HasMaxLength(1000);
        }
    }

    public class DesignTimeVerificationDbContextFactory : IDesignTimeDbContextFactory<ServiceDbContext>
    {
        public ServiceDbContext CreateDbContext(string[] args)
        {
            return new ServiceDbContext(new DbContextOptionsBuilder().UseMySql("Database='gooios_user_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true").Options);
        }
    }
}
