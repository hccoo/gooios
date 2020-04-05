using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Gooios.Infrastructure;
using Gooios.Infrastructure.Transactions;

namespace Gooios.UserService.Interceptors
{
    public class TransactionInterceptor : AbstractInterceptorAttribute
    {
        
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                var enableTransaction = false;
                IDbUnitOfWork dbUnitOfWork =  null;
                
                if (context.ImplementationMethod.IsDefined(typeof(EnableTransactionAttribute), false))
                {
                    enableTransaction = true;
                    var custAttr = (EnableTransactionAttribute)GetCustomAttribute(context.ImplementationMethod, typeof(EnableTransactionAttribute));
                    if (custAttr == null) throw new CanNotGetAttributeException(500, "Occur a exception when get the attribute from the target method.(Can not get the attribute named \"EnableTransactionAttribute\")");
                    BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
                    dbUnitOfWork = (IDbUnitOfWork)(context.Implementation.GetType().GetField("_dbUnitOfWork", flag).GetValue(context.Implementation));
                }
                using (var scope = new TransactionContext(dbUnitOfWork,enableTransaction))
                {
                    await next(context);
                    scope.Commit();
                }
            }
            catch (Exception ex)
            {
                //Log the exception
                throw;
            }
        }
    }
}
