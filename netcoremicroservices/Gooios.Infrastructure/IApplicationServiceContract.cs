using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.Infrastructure
{
    public interface IApplicationServiceContract : IDisposable
    {
    }

    public class ApplicationServiceContract : DisposableObject
    {
        protected override void Dispose(bool disposing)
        {
        }
    }
}
