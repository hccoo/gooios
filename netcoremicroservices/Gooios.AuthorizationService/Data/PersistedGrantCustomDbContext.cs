using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Gooios.AuthorizationService.Core;

namespace Gooios.AuthorizationService.Data
{
    public class PersistedGrantCustomDbContext : DbContext, IPersistedGrantDbContext
    {
        public PersistedGrantCustomDbContext(DbContextOptions<PersistedGrantCustomDbContext> options) : base(options)
        {
        }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PersistedGrantsConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql("Database='gooios_auth_db';Data Source='localhost';Port=3306;User Id='root';Password='111111';charset='utf8';pooling=true");
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }

    public class PersistedGrantCustomDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantCustomDbContext>
    {
        public PersistedGrantCustomDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PersistedGrantCustomDbContext>();

            return new PersistedGrantCustomDbContext(builder.Options);
        }
    }

    public class PersistedGrantsConfiguration : IEntityTypeConfiguration<PersistedGrant>
    {
        public PersistedGrantsConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<PersistedGrant> builder)
        {
            builder.ToTable<PersistedGrant>("persisted_grants");
            builder.HasKey(c => c.Key);

            builder.Property(c => c.ClientId).HasColumnName("client_id");
            builder.Property(c => c.CreationTime).HasColumnName("creation_time");
            builder.Property(c => c.Data).HasColumnName("data");
            builder.Property(c => c.Expiration).HasColumnName("expiration");
            builder.Property(c => c.Key).HasColumnName("key");
            builder.Property(c => c.SubjectId).HasColumnName("subject_id");
            builder.Property(c => c.Type).HasColumnName("type");

        }
    }
}

