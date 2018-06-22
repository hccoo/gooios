using Gooios.Infrastructure;
using Gooios.GoodsService.Configurations;
using Gooios.GoodsService.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using Gooios.GoodsService.Repositories;

namespace Gooios.GoodsService.Repositories
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
            //Console.WriteLine("DbProvider>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> " + _provider.GetHashCode());
            Console.WriteLine("DbContext>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> " + DbContext.GetHashCode());
            DbContext.SaveChanges();
        }
    }
}
