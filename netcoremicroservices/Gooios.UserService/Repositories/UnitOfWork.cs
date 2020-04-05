using Gooios.Infrastructure;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Gooios.UserService.Repositories
{
    public class DbUnitOfWork : IDbUnitOfWork
    {
        IDbContextProvider _provider;

        bool _disposed = false;
        bool _enableTransaction = false;
        ServiceDbContext _dbContext = null;
        IDbContextTransaction _transaction = null;

        public DbUnitOfWork(IDbContextProvider provider)
        {
            _provider = provider;
        }

        public DbContext DatabaseContext
        {
            get
            {
                return _dbContext ?? (_dbContext = _provider.GetDbContext());
            }
        }

        public ServiceDbContext DbContext => DatabaseContext as ServiceDbContext;

        public void BeginTransaction()
        {
            _transaction = DbContext.Database.BeginTransaction();
            _enableTransaction = true;
        }

        public virtual void Commit()
        {
            DbContext.SaveChanges();
        }

        public void Complete()
        {
            if (_enableTransaction)
            {
                _transaction.Commit();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);//通知垃圾回收器不调用对象终结器(析构器)
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    //在这里释放托管资源
                    if (this._transaction != null)
                    {
                        this._transaction.Dispose();
                        this._transaction = null;
                        _enableTransaction = false;
                    }
                    _disposed = true;
                }
                //在这里释放非托管资源
            }
            //disposed = true;
        }

        ///析构器在垃圾回收或程序终止时调用
        ~DbUnitOfWork()
        {
            Dispose(false);//释放非托管资源
        }
    }
}
