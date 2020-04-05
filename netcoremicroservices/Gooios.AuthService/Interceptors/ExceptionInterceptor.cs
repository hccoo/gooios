using AspectCore.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace Gooios.UserService.Interceptors
{
    public class ExceptionInterceptor : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex)
            {
                //Log the exception
                throw;
            }
        }
    }
}
