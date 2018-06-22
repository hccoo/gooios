using Gooios.OrganizationService;
using Gooios.OrganizationService.Configurations;
using Gooios.OrganizationService.Domains.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.OrganizationService.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<Organization> Organizations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new OrganizationConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = IocProvider.GetService<IServiceConfigurationProxy>()?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(conn);
        }
    }

    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("organizations");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.FullName).HasColumnName("full_name").HasMaxLength(200).IsRequired();
            builder.Property(c => c.ShortName).HasColumnName("short_name").HasMaxLength(80);
            builder.Property(c => c.Phone).HasColumnName("phone").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CustomServicePhone).HasColumnName("custom_service_phone").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.LastUpdBy).HasColumnName("updated_by").HasMaxLength(80);
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.LastUpdOn).HasColumnName("updated_on");
            builder.Property(c => c.Introduction).HasColumnName("introduction").HasMaxLength(4000);
            builder.Property(c => c.Latitude).HasColumnName("latitude");
            builder.Property(c => c.Longitude).HasColumnName("longitude");
            builder.Property(c => c.Province).HasColumnName("province").HasMaxLength(20).IsRequired();
            builder.Property(c => c.StreetAddress).HasColumnName("street_address").HasMaxLength(200).IsRequired();
            builder.Property(c => c.LogoImageId).HasColumnName("logo_image_id").HasMaxLength(200).IsRequired();
            builder.Property(c => c.Area).HasColumnName("area").HasMaxLength(20).IsRequired();
            builder.Property(c => c.City).HasColumnName("city").HasMaxLength(20).IsRequired();
            builder.Property(c => c.IsSuspend).HasColumnName("is_suspend").IsRequired();
            builder.Property(c => c.CertificateNo).HasColumnName("certificate_no").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Postcode).HasColumnName("post_code").HasMaxLength(20).IsRequired();
        }
    }

    public class DesignTimeCareermenDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            return new DatabaseContext(new DbContextOptionsBuilder().UseMySql("Database='gooios_organization_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true").Options);
        }
    }
}
