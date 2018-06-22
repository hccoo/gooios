using Gooios.Infrastructure;
using Gooios.VerificationService.Configurations;
using Gooios.VerificationService.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gooios.VerificationService.Repositories
{
    public class DbUnitOfWork : IDbUnitOfWork
    {
        IDbContextProvider _provider;

        bool _disposed = false;
        VerificationDbContext _dbContext = null;

        public DbUnitOfWork(IDbContextProvider provider)
        {
            _provider = provider;
        }

        public VerificationDbContext DbContext
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
