using Gooios.GoodsService;
using Gooios.GoodsService.Configurations;
using Gooios.GoodsService.Domains.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.GoodsService.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<Goods> Goods { get; set; }
        public DbSet<GoodsImage> GoodsImages { get; set; }
        public DbSet<GrouponCondition> GrouponConditions { get; set; }
        public DbSet<OnlineGoods> OnlineGoods { get; set; }
        public DbSet<OnlineGoodsImage> OnlineGoodsImages { get; set; }
        public DbSet<OnlineGrouponCondition> OnlineGrouponConditions { get; set; }
        public DbSet<GoodsCategory> GoodsCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentImage> CommentImages { get; set; }
        public DbSet<CommentTag> CommentTag { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new GoodsConfiguration());
            builder.ApplyConfiguration(new GoodsImageConfiguration());
            builder.ApplyConfiguration(new GrouponConditionConfiguration());
            builder.ApplyConfiguration(new OnlineGoodsConfiguration());
            builder.ApplyConfiguration(new OnlineGoodsImageConfiguration());
            builder.ApplyConfiguration(new OnlineGrouponConditionConfiguration());
            builder.ApplyConfiguration(new GoodsCategoryConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new CommentTagConfiguration());
            builder.ApplyConfiguration(new CommentImageConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = IocProvider.GetService<IServiceConfigurationProxy>()?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(conn);
        }
    }

    public class GoodsConfiguration : IEntityTypeConfiguration<Goods>
    {
        public void Configure(EntityTypeBuilder<Goods> builder)
        {
            builder.ToTable("goods");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.LastUpdBy).HasColumnName("updated_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.LastUpdOn).HasColumnName("updated_on").IsRequired();
            builder.Property(c => c.Description).HasColumnName("description").HasMaxLength(4000);
            builder.Property(c => c.VideoPath).HasColumnName("video_path").HasMaxLength(4000);
            builder.Property(c => c.Detail).HasColumnName("detail");
            builder.Property(c => c.ItemNumber).HasColumnName("item_number").HasMaxLength(80).IsRequired();
            builder.Property(c => c.MarketPrice).HasColumnName("market_price");
            builder.Property(c => c.OptionalPropertyJsonObject).HasColumnName("optional_property_json_object");
            builder.Property(c => c.Status).HasColumnName("status").IsRequired();
            builder.Property(c => c.Stock).HasColumnName("stock").IsRequired();
            builder.Property(c => c.StoreId).HasColumnName("store_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.Unit).HasColumnName("unit").IsRequired();
            builder.Property(c => c.UnitPrice).HasColumnName("unit_price").IsRequired();
            builder.Property(c => c.Category).HasColumnName("category").HasMaxLength(80).IsRequired();
            builder.Property(c => c.SubCategory).HasColumnName("sub_category").HasMaxLength(80);

            builder.Property(c => c.Province).HasColumnName("province").HasMaxLength(80).IsRequired();
            builder.Property(c => c.City).HasColumnName("city").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Area).HasColumnName("area").HasMaxLength(80).IsRequired();
            builder.Property(c => c.StreetAddress).HasColumnName("street_address").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Postcode).HasColumnName("post_code").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Latitude).HasColumnName("latitude").IsRequired();
            builder.Property(c => c.Longitude).HasColumnName("longitude").IsRequired();
            builder.Property(c => c.ApplicationId).HasColumnName("application_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.DistributionScope).HasColumnName("distribution_scope").HasMaxLength(80).IsRequired();
        }
    }

    public class GoodsImageConfiguration : IEntityTypeConfiguration<GoodsImage>
    {
        public void Configure(EntityTypeBuilder<GoodsImage> builder)
        {
            builder.ToTable("goods_images");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.GoodsId).HasColumnName("goods_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.ImageId).HasColumnName("image_id").IsRequired().HasMaxLength(80);
        }
    }

    public class GrouponConditionConfiguration : IEntityTypeConfiguration<GrouponCondition>
    {
        public void Configure(EntityTypeBuilder<GrouponCondition> builder)
        {
            builder.ToTable("groupon_conditions");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.MoreThanNumber).HasColumnName("more_than_number").IsRequired();
            builder.Property(c => c.GoodsId).HasColumnName("goods_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.Price).HasColumnName("price").IsRequired();
        }
    }
    
    public class OnlineGoodsConfiguration : IEntityTypeConfiguration<OnlineGoods>
    {
        public void Configure(EntityTypeBuilder<OnlineGoods> builder)
        {
            builder.ToTable("online_goods");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
            builder.Property(c => c.RecommendLevel).HasColumnName("recommend_level").HasDefaultValue(0);
            builder.Property(c => c.Order).HasColumnName("order").HasDefaultValue(0);
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.LastUpdBy).HasColumnName("updated_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.LastUpdOn).HasColumnName("updated_on").IsRequired();
            builder.Property(c => c.Description).HasColumnName("description").HasMaxLength(4000);
            builder.Property(c => c.VideoPath).HasColumnName("video_path").HasMaxLength(4000);
            builder.Property(c => c.Detail).HasColumnName("detail");
            builder.Property(c => c.ItemNumber).HasColumnName("item_number").HasMaxLength(80).IsRequired();
            builder.Property(c => c.MarketPrice).HasColumnName("market_price");
            builder.Property(c => c.OptionalPropertyJsonObject).HasColumnName("optional_property_json_object");
            builder.Property(c => c.Status).HasColumnName("status").IsRequired();
            builder.Property(c => c.Stock).HasColumnName("stock").IsRequired();
            builder.Property(c => c.StoreId).HasColumnName("store_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.Unit).HasColumnName("unit").IsRequired();
            builder.Property(c => c.UnitPrice).HasColumnName("unit_price").IsRequired();

            builder.Property(c => c.Province).HasColumnName("province").HasMaxLength(80).IsRequired();
            builder.Property(c => c.City).HasColumnName("city").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Area).HasColumnName("area").HasMaxLength(80).IsRequired();
            builder.Property(c => c.StreetAddress).HasColumnName("street_address").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Postcode).HasColumnName("post_code").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Latitude).HasColumnName("latitude").IsRequired();
            builder.Property(c => c.Longitude).HasColumnName("longitude").IsRequired();
            builder.Property(c => c.ApplicationId).HasColumnName("application_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.DistributionScope).HasColumnName("distribution_scope").HasMaxLength(80).IsRequired();
        }
    }

    public class OnlineGoodsImageConfiguration : IEntityTypeConfiguration<OnlineGoodsImage>
    {
        public void Configure(EntityTypeBuilder<OnlineGoodsImage> builder)
        {
            builder.ToTable("online_goods_images");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.GoodsId).HasColumnName("goods_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.ImageId).HasColumnName("image_id").IsRequired().HasMaxLength(80);
        }
    }

    public class OnlineGrouponConditionConfiguration : IEntityTypeConfiguration<OnlineGrouponCondition>
    {
        public void Configure(EntityTypeBuilder<OnlineGrouponCondition> builder)
        {
            builder.ToTable("online_groupon_conditions");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.MoreThanNumber).HasColumnName("more_than_number").IsRequired();
            builder.Property(c => c.GoodsId).HasColumnName("goods_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.Price).HasColumnName("price").IsRequired();
        }
    }
    
    public class GoodsCategoryConfiguration : IEntityTypeConfiguration<GoodsCategory>
    {
        public void Configure(EntityTypeBuilder<GoodsCategory> builder)
        {
            builder.ToTable("goods_categories");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Name).HasColumnName("name").IsRequired().HasMaxLength(80);
            builder.Property(c => c.Icon).HasColumnName("icon").HasMaxLength(800);
            builder.Property(c => c.ParentId).HasColumnName("parent_id").HasMaxLength(80);
            builder.Property(c => c.ApplicationId).HasColumnName("application_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Order).HasColumnName("order").IsRequired();
        }
    }

    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("tags");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Name).HasColumnName("name").IsRequired().HasMaxLength(80);
            builder.Property(c => c.CategoryId).HasColumnName("category_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").IsRequired().HasMaxLength(80);
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
        }
    }

    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comments");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Content).HasColumnName("content").IsRequired().HasMaxLength(80);
            builder.Property(c => c.GoodsId).HasColumnName("goods_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.OrderId).HasColumnName("order_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").IsRequired().HasMaxLength(80);
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
        }
    }

    public class CommentImageConfiguration : IEntityTypeConfiguration<CommentImage>
    {
        public void Configure(EntityTypeBuilder<CommentImage> builder)
        {
            builder.ToTable("comment_images");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CommentId).HasColumnName("comment_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.ImageId).HasColumnName("image_id").IsRequired().HasMaxLength(80);
        }
    }

    public class CommentTagConfiguration : IEntityTypeConfiguration<CommentTag>
    {
        public void Configure(EntityTypeBuilder<CommentTag> builder)
        {
            builder.ToTable("comment_tags");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CommentId).HasColumnName("comment_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.GoodsId).HasColumnName("goods_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.TagId).HasColumnName("tag_id").IsRequired().HasMaxLength(80);
            builder.Property(c => c.UserId).HasColumnName("user_id").IsRequired().HasMaxLength(80);
        }
    }

    public class DesignTimeCareermenDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            return new DatabaseContext(new DbContextOptionsBuilder().UseMySql("Database='gooios_goods_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true").Options);
        }
    }
}
