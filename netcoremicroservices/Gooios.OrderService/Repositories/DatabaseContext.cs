using Gooios.OrderService;
using Gooios.OrderService.Configurations;
using Gooios.OrderService.Domains.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gooios.OrderService.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<DeliveryNote> DeliveryNotes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderTrace> OrderTraces { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new DeliveryNoteConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderItemConfiguration());
            builder.ApplyConfiguration(new OrderTraceConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = IocProvider.GetService<IServiceConfigurationProxy>()?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(conn);
        }
    }

    public class DeliveryNoteConfiguration : IEntityTypeConfiguration<DeliveryNote>
    {
        public void Configure(EntityTypeBuilder<DeliveryNote> builder)
        {
            builder.ToTable("delivery_notes");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CarrierName).HasColumnName("carrier_name").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.CarrierPhone).HasColumnName("carrier_phone").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Consignee).HasColumnName("consignee").IsRequired().HasMaxLength(80);
            builder.Property(c => c.ConsigneeMobile).HasColumnName("consignee_mobile").IsRequired().HasMaxLength(20);
            builder.Property(c => c.DeliveryAddress).HasColumnName("delivery_address").IsRequired().HasMaxLength(80);
            builder.Property(c => c.DeliveryNoteNo).HasColumnName("delivery_note_no").HasMaxLength(80).IsRequired();
            builder.Property(c => c.OrderId).HasColumnName("order_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ShippingAmount).HasColumnName("shipping_amount").IsRequired();
            builder.Property(c => c.ShippingMethod).HasColumnName("shipping_method").IsRequired();
        }
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.CustomerMobile).HasColumnName("customer_mobile").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CustomerName).HasColumnName("customer_name").IsRequired().HasMaxLength(80);
            builder.Property(c => c.InvoiceType).HasColumnName("invoice_type").IsRequired();
            builder.Property(c => c.OrderNo).HasColumnName("order_no").IsRequired().HasMaxLength(80);
            builder.Property(c => c.Title).HasColumnName("title").HasMaxLength(200);
            builder.Property(c => c.PayAmount).HasColumnName("pay_amount").IsRequired();
            builder.Property(c => c.PreferentialAmount).HasColumnName("preferential_amount").IsRequired();
            builder.Property(c => c.ShippingCost).HasColumnName("shipping_cost").IsRequired();
            builder.Property(c => c.Status).HasColumnName("status").IsRequired();
            builder.Property(c => c.Tax).HasColumnName("tax").IsRequired();
            builder.Property(c => c.TotalAmount).HasColumnName("total_amount").IsRequired();
            builder.Property(c => c.UpdatedBy).HasColumnName("updated_by").IsRequired().HasMaxLength(80);
            builder.Property(c => c.UpdatedOn).HasColumnName("updated_on").IsRequired();
            builder.Property(c => c.Province).HasColumnName("province").IsRequired().HasMaxLength(80);
            builder.Property(c => c.City).HasColumnName("city").IsRequired().HasMaxLength(80);
            builder.Property(c => c.Area).HasColumnName("area").IsRequired().HasMaxLength(80);
            builder.Property(c => c.StreetAddress).HasColumnName("street_address").IsRequired().HasMaxLength(200);
            builder.Property(c => c.Postcode).HasColumnName("post_code").IsRequired().HasMaxLength(80);
            builder.Property(c => c.Mark).HasColumnName("mark").IsRequired().HasMaxLength(80);
            builder.Property(c => c.InvoiceRemark).HasColumnName("invoice_remark").HasMaxLength(500);
            builder.Property(c => c.Remark).HasColumnName("remark").HasMaxLength(500);
            builder.Property(c => c.OrganizationId).HasColumnName("organization_id").IsRequired().HasMaxLength(80); 
            builder.Property(c => c.ActivityId).HasColumnName("activity_id").HasMaxLength(80);
        }
    }

    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.Count).HasColumnName("count").IsRequired();
            builder.Property(c => c.ObjectId).HasColumnName("object_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.ObjectNo).HasColumnName("object_no").HasMaxLength(80).IsRequired();
            builder.Property(c => c.OrderId).HasColumnName("order_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.PreviewPictureUrl).HasColumnName("preview_picture_url").HasMaxLength(200).IsRequired();
            builder.Property(c => c.SelectedProperties).HasColumnName("selected_properties").IsRequired().HasMaxLength(2000);
            builder.Property(c => c.Title).HasColumnName("title").IsRequired().HasMaxLength(200);
            builder.Property(c => c.TradeUnitPrice).HasColumnName("trade_unit_price").IsRequired();
        }
    }

    public class OrderTraceConfiguration : IEntityTypeConfiguration<OrderTrace>
    {
        public void Configure(EntityTypeBuilder<OrderTrace> builder)
        {
            builder.ToTable("order_traces");
            builder.HasKey(c => new { c.Id });

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedOn).HasColumnName("created_on").IsRequired();
            builder.Property(c => c.IsSuccess).HasColumnName("is_success").IsRequired();
            builder.Property(c => c.OrderId).HasColumnName("order_id").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Status).HasColumnName("status").IsRequired();
        }
    }

    public class DesignTimeCareermenDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            return new DatabaseContext(new DbContextOptionsBuilder().UseMySql("Database='gooios_order_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true").Options);
        }
    }
}
