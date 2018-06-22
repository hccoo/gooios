using Gooios.VerificationService.Configurations;
using Gooios.VerificationService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.VerificationService.Repositories
{
    public class VerificationDbContext : DbContext
    {
        public VerificationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Verification> Verifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyConfiguration(new VerificationConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = IocProvider.GetService<IServiceConfigurationProxy>()?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(conn);
                //optionsBuilder.UseMySql("Database=gooios_auth_db;Data Source=localhost;Port=3306;User Id=root;Password=111111;charset=utf8;pooling=true");
        }
    }

    public class VerificationConfiguration : IEntityTypeConfiguration<Verification>
    {
        public void Configure(EntityTypeBuilder<Verification> builder)
        {
            builder.ToTable("verifications");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.BizCode).HasColumnName("biz_code").IsRequired();
            builder.Property(c => c.Code).HasColumnName("code").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.ExpiredOn).HasColumnName("expired_on").IsRequired();
            builder.Property(c => c.IsSuspend).HasColumnName("is_suspend").IsRequired();
            builder.Property(c => c.IsUsed).HasColumnName("is_used").IsRequired();
            builder.Property(c => c.LastUpdOn).HasColumnName("last_upd_on");
            builder.Property(c => c.To).HasColumnName("to").HasMaxLength(80).IsRequired();
        }
    }

    public class DesignTimeVerificationDbContextFactory : IDesignTimeDbContextFactory<VerificationDbContext>
    {
        public VerificationDbContext CreateDbContext(string[] args)
        {
            return new VerificationDbContext(new DbContextOptionsBuilder().UseMySql("Database='gooios_verification_db';Data Source='localhost';Port=3306;User Id='root';Password='!QAZ2wsx';charset='utf8';pooling=true").Options);
        }
    }
}
