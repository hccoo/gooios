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

            if (context.IsAsync())
            {
                if (context.ServiceMethod.ReturnType.FullName == "System.Threading.Tasks.Task")
                {
                    logger.Info($"Result value: void");
                }
                else
                {
                    var result = await context.UnwrapAsyncReturnValue();
                    var res = JsonConvert.SerializeObject(result);
                    logger.Info($"Result value: {res}");
                }
            }
            else
            {
                if (context.ReturnValue != null)
                {
                    var res = JsonConvert.SerializeObject(context.ReturnValue);
                    logger.Info($"Result value: {res}");
                }
            }
        }
    }
}
