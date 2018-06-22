using Gooios.Infrastructure;
using Gooios.PartnerGateway.Configurations;
using Gooios.PartnerGateway.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using Gooios.PartnerGateway.Repositories;

namespace Gooios.PartnerGateway.Repositories
{
    public class DbUnitOfWork : IDbUnitOfWork
    {
        IDbContextProvider _provider;

        bool _disposed = false;
        DatabaseContext _dbContext = null;

        public DbUnitOfWork(IDbContextProvider provider)
        {
            _provider = provider;
        }

        public DatabaseContext DbContext
        {
            get
            {
                return _dbContext ?? (_dbContext = _provider.GetDbContext());
            }
        }

        public virtual void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
