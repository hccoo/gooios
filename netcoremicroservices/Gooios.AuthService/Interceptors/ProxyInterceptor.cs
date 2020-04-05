using AspectCore.DynamicProxy;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gooios.UserService.Interceptors
{
    public class ProxyInterceptor : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var logger = LogManager.GetLogger("ProxyInterceptor");

            var parameters = JsonConvert.SerializeObject(context.Parameters);
            logger.Info($"Call api {context.ServiceMethod.DeclaringType.FullName}->{context.ServiceMethod.Name} Paramters：{parameters}");

            await next(context);
            
            var res = JsonConvert.SerializeObject(await context.UnwrapAsyncReturnValue());
            logger.Info($"Result value: {res}");
        }
    }
}
