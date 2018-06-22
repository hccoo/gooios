using Gooios.Infrastructure;
using Gooios.PaymentService.Configurations;
using Gooios.PaymentService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using Gooios.PaymentService.Repositories;

namespace Gooios.PaymentService.Repositories
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
