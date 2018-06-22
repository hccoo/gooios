using Gooios.ImageService.Configurations;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gooios.ImageService.Repositories
{
    public interface IDbContextProvider : IDisposable
    {
        DatabaseContext GetDbContext();
    }

    public class DbContextProvider : IDbContextProvider
    {
        private DatabaseContext _dataContext;
        private IServiceConfigurationProxy _configuration;

        bool _disposed = false;

        public DbContextProvider(IServiceConfigurationProxy configuration)
        {
            _configuration = configuration;
        }

        public DatabaseContext GetDbContext()
        {
            return _dataContext ?? (_dataContext = new DatabaseContext(new DbContextOptionsBuilder().UseMySql(_configuration.ConnectionString).Options));
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
                    if (this._dataContext != null)
                    {
                        this._dataContext.Dispose();
                        this._dataContext = null;
                    }
                    _disposed = true;
                }
                //在这里释放非托管资源
            }
            //disposed = true;
        }

        ///析构器在垃圾回收或程序终止时调用
        ~DbContextProvider()
        {
            Dispose(false);//释放非托管资源
        }
    }
}
