using Gooios.PaymentGateway;
using Gooios.PaymentGateway.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.PaymentGateway.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        //public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            //builder.ApplyConfiguration(new ImageConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = IocProvider.GetService<IServiceConfigurationProxy>()?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(conn);
        }
    }

    //public class ImageConfiguration : IEntityTypeConfiguration<Image>
    //{
    //    public void Configure(EntityTypeBuilder<Image> builder)
    //    {
    //        builder.ToTable("images");
    //        builder.HasKey(c => new { c.Id });

    //        builder.Property(c => c.Id).HasColumnName("id");
    //        builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
    //        builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
    //        builder.Property(c => c.Description).HasColumnName("description").HasMaxLength(500).IsRequired();
    //        builder.Property(c => c.HttpPath).HasColumnName("http_path").HasMaxLength(500).IsRequired();
    //        builder.Property(c => c.Title).HasColumnName("title").HasMaxLength(80).IsRequired();
    //    }
    //}

    public class DesignTimeCareermenDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            return new DatabaseContext(new DbContextOptionsBuilder().UseMySql("Database='gooios_image_db';Data Source='localhost';Port=3306;User Id='root';Password='111111';charset='utf8';pooling=true").Options);
        }
    }
}
