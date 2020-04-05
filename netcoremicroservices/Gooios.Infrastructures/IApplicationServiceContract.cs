using System;

namespace Gooios.Infrastructure
{
    public interface IApplicationServiceContract : IDisposable
    {
    }

    public class ApplicationServiceContract : DisposableObject
    {
        protected readonly IDbUnitOfWork _dbUnitOfWork;

        public ApplicationServiceContract(IDbUnitOfWork dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
