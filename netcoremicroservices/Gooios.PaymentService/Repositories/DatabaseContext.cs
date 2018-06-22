using Gooios.PaymentService;
using Gooios.PaymentService.Configurations;
using Gooios.PaymentService.Domains.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.PaymentService.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<WeChatPaymentNotifyMessage> WeChatPaymentNotifyMessages { get; set; }
        public DbSet<WeChatAppConfiguration> WeChatAppConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new WeChatPaymentNotifyMessageConfiguration());
            builder.ApplyConfiguration(new WeChatAppConfigurationConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = IocProvider.GetService<IServiceConfigurationProxy>()?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(conn);
        }
    }

    public class WeChatPaymentNotifyMessageConfiguration : IEntityTypeConfiguration<WeChatPaymentNotifyMessage>
    {
        public void Configure(EntityTypeBuilder<WeChatPaymentNotifyMessage> builder)
        {
            builder.ToTable("wechat_payment_notify_messages");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.MessageContent).HasColumnName("message_content").IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.NotifyApiName).HasColumnName("notify_api_name").HasMaxLength(80).IsRequired();
        }
    }

    public class WeChatAppConfigurationConfiguration : IEntityTypeConfiguration<WeChatAppConfiguration>
    {
        public void Configure(EntityTypeBuilder<WeChatAppConfiguration> builder)
        {
            builder.ToTable("wechat_app_configurations");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.AppId).HasColumnName("app_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.MchId).HasColumnName("mch_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Key).HasColumnName("key").HasMaxLength(80).IsRequired();
            builder.Property(c => c.AppSecret).HasColumnName("app_secret").HasMaxLength(80).IsRequired();
            builder.Property(c => c.SslCertPassword).HasColumnName("sslcert_password").HasMaxLength(80).IsRequired();
            builder.Property(c => c.SslCertPath).HasColumnName("sslcert_path").HasMaxLength(200).IsRequired();
            builder.Property(c => c.NotifyUrl).HasColumnName("notify_url").HasMaxLength(200).IsRequired();
            builder.Property(c => c.OrganizationId).HasColumnName("organization_id").HasMaxLength(80).IsRequired();
        }
    }

    public class DesignTimeCareermenDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            return new DatabaseContext(new DbContextOptionsBuilder().UseMySql("Database='gooios_paymentservice_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true").Options);
        }
    }
}
