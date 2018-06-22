using System;
using System.Collections.Generic;

namespace Gooios.Infrastructure.Transactions
{
    public interface ITransactionCoordinator : ITransactionUnitOfWork, IDisposable
    {
    }

    public sealed class TransactionCoordinator : DisposableObject, ITransactionCoordinator
    {
        private bool disposed = false;
        private readonly List<IUnitOfWork> managedUnitOfWorks = new List<IUnitOfWork>();

        public TransactionCoordinator(params IUnitOfWork[] unitOfWorks)
        {
            if (unitOfWorks != null &&
                unitOfWorks.Length > 0)
            {
                foreach (var uow in unitOfWorks)
                    managedUnitOfWorks.Add(uow);
            }
        }
        
        #region IUnitOfWork Members

        public void Commit()
        {
            if (managedUnitOfWorks.Count > 0)
                foreach (var uow in managedUnitOfWorks)
                    uow.Commit();
        }

        #endregion


        protected override void Dispose(bool disposing)
        {
        }
    }
}
