using Gooios.LogService.Configurations;
using Gooios.LogService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.LogService.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<SystemLog> SystemLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyConfiguration(new SystemLogConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = IocProvider.GetService<IServiceConfigurationProxy>()?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(conn);
        }
    }

    public class SystemLogConfiguration : IEntityTypeConfiguration<SystemLog>
    {
        public void Configure(EntityTypeBuilder<SystemLog> builder)
        {
            builder.ToTable("system_logs");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.ApplicationKey).HasColumnName("application_key").HasMaxLength(80).IsRequired();
            builder.Property(c => c.AppServiceName).HasColumnName("app_service_name").HasMaxLength(80).IsRequired();
            builder.Property(c => c.BizData).HasColumnName("biz_data").HasMaxLength(2000).IsRequired();
            builder.Property(c => c.CallerApplicationKey).HasColumnName("caller_application_key").HasMaxLength(80);
            builder.Property(c => c.Exception).HasColumnName("exception").HasMaxLength(2000);
            builder.Property(c => c.Level).HasColumnName("level").IsRequired();
            builder.Property(c => c.LogThread).HasColumnName("log_thread").IsRequired();
            builder.Property(c => c.LogTime).HasColumnName("log_time").IsRequired();
            builder.Property(c => c.Operation).HasColumnName("operation").HasMaxLength(200).IsRequired();
            builder.Property(c => c.ReturnValue).HasColumnName("return_value").HasMaxLength(2000);

        }
    }

    public class DesignTimeCareermenDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            return new DatabaseContext(new DbContextOptionsBuilder().UseMySql("Database='gooios_log_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true").Options);
        }
    }
}
