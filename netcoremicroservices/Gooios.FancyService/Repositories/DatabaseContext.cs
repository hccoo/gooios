using Gooios.FancyService.Configurations;
using Gooios.FancyService.Domains.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.FancyService.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CommentImage> CommentImages { get; set; }
        public DbSet<CommentTag> CommentTags { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceImage> ServiceImages { get; set; }
        public DbSet<Servicer> Servicers { get; set; }
        public DbSet<ServicerImage> ServicerImages { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new CommentImageConfiguration());
            builder.ApplyConfiguration(new CommentTagConfiguration());
            builder.ApplyConfiguration(new ReservationConfiguration());
            builder.ApplyConfiguration(new ServiceConfiguration());
            builder.ApplyConfiguration(new ServiceImageConfiguration());
            builder.ApplyConfiguration(new ServicerImageConfiguration());
            builder.ApplyConfiguration(new ServicerConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = IocProvider.GetService<IServiceConfigurationProxy>()?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(conn);
        }
    }

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Mark).HasColumnName("mark").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ApplicationId).HasColumnName("application_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ParentId).HasColumnName("parent_id").HasMaxLength(80);
            builder.Property(c => c.Order).HasColumnName("order").IsRequired();

        }
    }

    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comments");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Content).HasColumnName("content").HasMaxLength(500).IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.OrderId).HasColumnName("order_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ReservationId).HasColumnName("reservation_id").HasMaxLength(80).IsRequired();
        }
    }

    public class CommentImageConfiguration : IEntityTypeConfiguration<CommentImage>
    {
        public void Configure(EntityTypeBuilder<CommentImage> builder)
        {
            builder.ToTable("comment_images");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CommentId).HasColumnName("comment_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ImageId).HasColumnName("image_id").HasMaxLength(80).IsRequired();
        }
    }

    public class CommentTagConfiguration : IEntityTypeConfiguration<CommentTag>
    {
        public void Configure(EntityTypeBuilder<CommentTag> builder)
        {
            builder.ToTable("comment_tags");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CommentId).HasColumnName("comment_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ReservationId).HasColumnName("reservation_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.TagId).HasColumnName("tag_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.UserId).HasColumnName("user_id").HasMaxLength(80).IsRequired();
        }
    }

    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("reservations");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.AppointTime).HasColumnName("appoint_time").IsRequired();
            builder.Property(c => c.Area).HasColumnName("area").HasMaxLength(80).IsRequired();
            builder.Property(c => c.City).HasColumnName("city").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.CustomerMobile).HasColumnName("customer_mobile").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CustomerName).HasColumnName("customer_name").HasMaxLength(80).IsRequired();
            builder.Property(c => c.LastUpdBy).HasColumnName("updated_by").HasMaxLength(80);
            builder.Property(c => c.LastUpdOn).HasColumnName("updated_on");
            builder.Property(c => c.Latitude).HasColumnName("latitude").IsRequired();
            builder.Property(c => c.Longitude).HasColumnName("longitude").IsRequired();
            builder.Property(c => c.Postcode).HasColumnName("postcode").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Province).HasColumnName("province").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ReservationNo).HasColumnName("reservation_no").HasMaxLength(80).IsRequired();
            //builder.Property(c => c.ServiceDestination).HasColumnName("service_destination").HasMaxLength(200).IsRequired();
            builder.Property(c => c.OrganizationId).HasColumnName("organization_id").HasMaxLength(80);
            builder.Property(c => c.ServiceId).HasColumnName("service_id").HasMaxLength(80);
            builder.Property(c => c.ServicerId).HasColumnName("servicer_id").HasMaxLength(80);
            builder.Property(c => c.SincerityGoldNeedToPay).HasColumnName("sincerity_gold_need_to_pay").IsRequired();
            builder.Property(c => c.Status).HasColumnName("status").IsRequired();
            builder.Property(c => c.StreetAddress).HasColumnName("street_address").HasMaxLength(200).IsRequired();
            builder.Property(c => c.OrderId).HasColumnName("order_id").HasMaxLength(80);
        }
    }

    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("services");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Area).HasColumnName("area").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Category).HasColumnName("category").HasMaxLength(80).IsRequired();
            builder.Property(c => c.City).HasColumnName("city").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.Introduction).HasColumnName("introduction").IsRequired();
            builder.Property(c => c.LastUpdBy).HasColumnName("updated_by").HasMaxLength(80);
            builder.Property(c => c.LastUpdOn).HasColumnName("updated_at").HasMaxLength(80);
            builder.Property(c => c.Latitude).HasColumnName("latitude").IsRequired();
            builder.Property(c => c.Longitude).HasColumnName("longitude").IsRequired();
            builder.Property(c => c.OrganizationId).HasColumnName("organization_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Postcode).HasColumnName("postcode").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Province).HasColumnName("province").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ServeScope).HasColumnName("serve_scope").IsRequired();
            builder.Property(c => c.SincerityGold).HasColumnName("sincerity_gold").IsRequired();
            builder.Property(c => c.Status).HasColumnName("status").IsRequired();
            builder.Property(c => c.StreetAddress).HasColumnName("street_address").HasMaxLength(80).IsRequired();
            builder.Property(c => c.SubCategory).HasColumnName("sub_category").HasMaxLength(80);
            builder.Property(c => c.Title).HasColumnName("title").HasMaxLength(80).IsRequired();
            builder.Property(c => c.IsAdvertisement).HasColumnName("is_advertisement").IsRequired();
            builder.Property(c => c.PersonalizedPageUri).HasColumnName("personalized_page_uri").HasMaxLength(500);
            builder.Property(c => c.ApplicationId).HasColumnName("application_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.VideoUrl).HasColumnName("video_url").HasMaxLength(1000);
            builder.Property(c => c.IOSVideoUrl).HasColumnName("ios_video_url").HasMaxLength(1000);
            builder.Property(c => c.GoodsCategoryName).HasColumnName("goods_category_name").HasMaxLength(200);
        }
    }

    public class ServiceImageConfiguration : IEntityTypeConfiguration<ServiceImage>
    {
        public void Configure(EntityTypeBuilder<ServiceImage> builder)
        {
            builder.ToTable("service_images");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.ServiceId).HasColumnName("service_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ImageId).HasColumnName("image_id").HasMaxLength(80).IsRequired();
        }
    }

    public class ServicerImageConfiguration : IEntityTypeConfiguration<ServicerImage>
    {
        public void Configure(EntityTypeBuilder<ServicerImage> builder)
        {
            builder.ToTable("servicer_images");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.ServicerId).HasColumnName("servicer_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ImageId).HasColumnName("image_id").HasMaxLength(80).IsRequired();
        }
    }

    public class ServicerConfiguration : IEntityTypeConfiguration<Servicer>
    {
        public void Configure(EntityTypeBuilder<Servicer> builder)
        {
            builder.ToTable("servicers");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Area).HasColumnName("area").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Category).HasColumnName("category").HasMaxLength(80).IsRequired();
            builder.Property(c => c.City).HasColumnName("city").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.BirthDay).HasColumnName("birth_day").IsRequired();
            builder.Property(c => c.LastUpdBy).HasColumnName("updated_by").HasMaxLength(80);
            builder.Property(c => c.LastUpdOn).HasColumnName("updated_at").HasMaxLength(80);
            builder.Property(c => c.Latitude).HasColumnName("latitude").IsRequired();
            builder.Property(c => c.Longitude).HasColumnName("longitude").IsRequired();
            builder.Property(c => c.OrganizationId).HasColumnName("organization_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Postcode).HasColumnName("postcode").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Province).HasColumnName("province").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Gender).HasColumnName("gender").IsRequired();
            builder.Property(c => c.IsSuspend).HasColumnName("is_suspend").IsRequired();
            builder.Property(c => c.JobNumber).HasColumnName("job_number").HasMaxLength(80).IsRequired();
            builder.Property(c => c.StreetAddress).HasColumnName("street_address").HasMaxLength(200).IsRequired();
            builder.Property(c => c.SubCategory).HasColumnName("sub_category").HasMaxLength(80);
            builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(80).IsRequired();
            builder.Property(c => c.PersonalIntroduction).HasColumnName("personal_introduction").IsRequired();
            builder.Property(c => c.PortraitImageId).HasColumnName("portrait_image_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.SincerityGoldRate).HasColumnName("sincerity_gold_rate").IsRequired();
            builder.Property(c => c.SincerityGold).HasColumnName("SincerityGold").IsRequired();
            builder.Property(c => c.StartRelevantWorkTime).HasColumnName("start_relevant_work_time").IsRequired();
            builder.Property(c => c.TechnicalGrade).HasColumnName("technical_grade").IsRequired();
            builder.Property(c => c.TechnicalTitle).HasColumnName("technical_title").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ApplicationId).HasColumnName("application_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.UserName).HasColumnName("user_name").HasMaxLength(80);
        }
    }

    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("tags");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CategoryId).HasColumnName("category_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
        }
    }

    public class DesignTimeCareermenDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            return new DatabaseContext(new DbContextOptionsBuilder().UseMySql("Database='gooios_fancyservice_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true").Options);
        }
    }
}
