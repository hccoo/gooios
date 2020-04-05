using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Gooios.AuthService.Data
{
    public class PersistedGrantCustomDbContext : DbContext, IPersistedGrantDbContext
    {
        public PersistedGrantCustomDbContext(DbContextOptions<PersistedGrantCustomDbContext> options) : base(options)
        {
        }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PersistedGrantsConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceFlowCodesConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql("Database='gooios_authservice_db';Data Source='localhost';Port=3306;User Id='root';Password='!Qaz2wSX';charset='utf8';pooling=true");
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
            builder.HasKey(c => c.Key);
        }
    }
    public class DeviceFlowCodesConfiguration : IEntityTypeConfiguration<DeviceFlowCodes>
    {
        public DeviceFlowCodesConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<DeviceFlowCodes> builder)
        {
            builder.HasKey(c => c.UserCode);
        }
    }
}

