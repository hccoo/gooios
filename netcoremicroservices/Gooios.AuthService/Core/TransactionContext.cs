using Gooios.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthService.Core
{
    public class TransactionContext : IDisposable
    {
        private readonly IDbUnitOfWork _dbUnitOfWork;
        private bool isEnableTransaction = false;
        private bool disposed = false;

        public TransactionContext(IDbUnitOfWork dbUnitOfWork, bool needTransaction = false)
        {
            if (needTransaction)
            {
                _dbUnitOfWork = dbUnitOfWork;
                _dbUnitOfWork.BeginTransaction();
                isEnableTransaction = true;
            }
        }

        public virtual void Commit()
        {
            if (isEnableTransaction)
            {
                _dbUnitOfWork.Complete();
            }
        }

        private void ClearUp(bool disposing)
        {
            if (isEnableTransaction)
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        this._dbUnitOfWork.Dispose();
                        disposed = true;
                    }
                }
            }
        }

        public void Dispose()
        {
            ClearUp(true);
            GC.SuppressFinalize(this);
        }

        ~TransactionContext()
        {
            ClearUp(false);
        }
    }
}
