using Gooios.Infrastructure;
using Gooios.ImageService.Configurations;
using Gooios.ImageService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using Gooios.ImageService.Repositories;

namespace Gooios.ImageService.Repositories
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
