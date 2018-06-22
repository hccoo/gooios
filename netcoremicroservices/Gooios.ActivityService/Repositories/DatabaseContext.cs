using Gooios.ActivityService;
using Gooios.ActivityService.Configurations;
using Gooios.ActivityService.Domains.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.ActivityService.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<GrouponActivity> GrouponActivities { get; set; }
        public DbSet<GrouponParticipation> GrouponParticipations { get; set; }
        public DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new GrouponActivityConfiguration());
            builder.ApplyConfiguration(new GrouponParticipationConfiguration());
            builder.ApplyConfiguration(new TopicConfiguration());
            builder.ApplyConfiguration(new TopicImageConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = IocProvider.GetService<IServiceConfigurationProxy>()?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(conn);
        }
    }

    public class GrouponActivityConfiguration : IEntityTypeConfiguration<GrouponActivity>
    {
        public void Configure(EntityTypeBuilder<GrouponActivity> builder)
        {
            builder.ToTable("groupon_activities");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Count).HasColumnName("count").IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.End).HasColumnName("end").IsRequired();
            builder.Property(c => c.LastUpdOn).HasColumnName("updated_on").IsRequired();
            builder.Property(c => c.ProductId).HasColumnName("product_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ProductMark).HasColumnName("product_mark").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Start).HasColumnName("start").IsRequired();
            builder.Property(c => c.Status).HasColumnName("status").IsRequired();
            builder.Property(c => c.UnitPrice).HasColumnName("unit_price").IsRequired();
        }
    }
    public class GrouponParticipationConfiguration : IEntityTypeConfiguration<GrouponParticipation>
    {
        public void Configure(EntityTypeBuilder<GrouponParticipation> builder)
        {
            builder.ToTable("groupon_participations");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.BuyCount).HasColumnName("buy_count").IsRequired();
            builder.Property(c => c.GrouponActivityId).HasColumnName("groupon_activity_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.OrderId).HasColumnName("order_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ParticipateTime).HasColumnName("participate_time").IsRequired();
            builder.Property(c => c.UserId).HasColumnName("user_id").HasMaxLength(80).IsRequired();
        }
    }
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.ToTable("topics");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.LastUpdBy).HasColumnName("updated_by").HasMaxLength(80);
            builder.Property(c => c.LastUpdOn).HasColumnName("updated_on").IsRequired();
            builder.Property(c => c.StreetAddress).HasColumnName("street_address").HasMaxLength(200).IsRequired();
            builder.Property(c => c.Area).HasColumnName("area").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Province).HasColumnName("province").HasMaxLength(80).IsRequired();
            builder.Property(c => c.City).HasColumnName("city").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Category).HasColumnName("category");
            builder.Property(c => c.CustomTopicUrl).HasColumnName("custom_topic_url").HasMaxLength(500);
            builder.Property(c => c.EndDate).HasColumnName("end_date");
            builder.Property(c => c.FaceImageUrl).HasColumnName("face_image_url").IsRequired().HasMaxLength(500);
            builder.Property(c => c.Introduction).HasColumnName("introduction").IsRequired();
            builder.Property(c => c.IsCustom).HasColumnName("is_custom").IsRequired();
            builder.Property(c => c.IsSuspend).HasColumnName("is_suspend").IsRequired();
            builder.Property(c => c.Latitude).HasColumnName("latitude").IsRequired();
            builder.Property(c => c.Longitude).HasColumnName("longitude").IsRequired();
            builder.Property(c => c.StartDate).HasColumnName("start_date");
            builder.Property(c => c.Title).HasColumnName("title").HasMaxLength(80);
            builder.Property(c => c.OrganizationId).HasColumnName("organization_id").HasMaxLength(80);
            builder.Property(c => c.Postcode).HasColumnName("postcode").HasMaxLength(80);
            builder.Property(c => c.CreatorName).HasColumnName("creator_name").HasMaxLength(80);
            builder.Property(c => c.CreatorPortraitUrl).HasColumnName("creator_portrait_url").HasMaxLength(500);
        }
    }
    public class TopicImageConfiguration : IEntityTypeConfiguration<TopicImage>
    {
        public void Configure(EntityTypeBuilder<TopicImage> builder)
        {
            builder.ToTable("topic_images");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.ImageId).HasColumnName("image_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ImageUrl).HasColumnName("image_url").HasMaxLength(500).IsRequired();
            builder.Property(c => c.TopicId).HasColumnName("topic_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Order).HasColumnName("order").IsRequired();
        }
    }

    public class DesignTimeCareermenDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            return new DatabaseContext(new DbContextOptionsBuilder().UseMySql("Database='gooios_activity_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true").Options);
        }
    }
}
