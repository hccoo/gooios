using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Threading.Tasks;

namespace Gooios.UserService.Interceptors
{
    public class ExceptionInterceptor : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            Logger logger = LogManager.GetLogger("ExceptionInterceptor");
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.InnerException?.Message ?? "");
                throw;
            }
        }
    }
}
