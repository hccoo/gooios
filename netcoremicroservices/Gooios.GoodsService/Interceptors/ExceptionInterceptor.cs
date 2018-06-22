using Castle.DynamicProxy;
using Gooios.Infrastructure.Exceptions;
using System;
using System.Threading.Tasks;

namespace Gooios.GooiosService.Interceptors
{
    public class ExceptionInterceptor : IInterceptor // AbstractInterceptorAttribute
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                //Log the exception
                throw;
            }
        }

        //public override async Task Invoke(AspectContext context, AspectDelegate next)
        //{
        //    try
        //    {
        //        await next(context);
        //    }
        //    catch(Exception ex)
        //    {
        //        //Log the exception
        //        throw;
        //    }
        //}
    }
}
