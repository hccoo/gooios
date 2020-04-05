using Gooios.Infrastructure;
using Gooios.UserService.Configurations;
using Gooios.UserService.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gooios.UserService.Repositories
{
    public class DbUnitOfWork : IDbUnitOfWork
    {
        IDbContextProvider _provider;

        bool _disposed = false;
        //UserDbContext _dbContext = null;

        public DbUnitOfWork(IDbContextProvider provider)
        {
            _provider = provider;
        }

        //public UserDbContext DbContext
        //{
        //    get
        //    {
        //        return _dbContext ?? (_dbContext = _provider.GetDbContext());
        //    }
        //}

        public virtual void Commit()
        {
            //DbContext.SaveChanges();
        }
    }
}
