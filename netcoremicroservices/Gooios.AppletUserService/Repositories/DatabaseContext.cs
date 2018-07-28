using Gooios.AppletUserService;
using Gooios.AppletUserService.Configurations;
using Gooios.AppletUserService.Domains.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.AppletUserService.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<UserSession> UserSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserSessionConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = IocProvider.GetService<IServiceConfigurationProxy>()?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(conn);
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.ApplicationId).HasColumnName("application_id").HasMaxLength(200).IsRequired();
            builder.Property(c => c.Channel).HasColumnName("channel").IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.NickName).HasColumnName("nickname").HasMaxLength(200).IsRequired();
            builder.Property(c => c.LastUpdOn).HasColumnName("updated_on");
            builder.Property(c => c.OpenId).HasColumnName("open_id").HasMaxLength(500);
            builder.Property(c => c.OrganizationId).HasColumnName("organization_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.UserId).HasColumnName("user_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.UserPortrait).HasColumnName("user_portrait").HasMaxLength(1000);
        }
    }

    public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.ToTable("user_sessions");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.ExpiredOn).HasColumnName("expired_on").IsRequired();
            builder.Property(c => c.GooiosSessionKey).HasColumnName("gooios_session_key").IsRequired().HasMaxLength(1000);
            builder.Property(c => c.OpenId).HasColumnName("open_id").HasMaxLength(500);
            builder.Property(c => c.SessionKey).HasColumnName("session_key").IsRequired().HasMaxLength(1000);
            builder.Property(c => c.UserId).HasColumnName("user_id").HasMaxLength(80).IsRequired();
        }
    }

    public class DesignTimeCareermenDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            return new DatabaseContext(new DbContextOptionsBuilder().UseMySql("Database='gooios_appletuser_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true").Options);
        }
    }
}
