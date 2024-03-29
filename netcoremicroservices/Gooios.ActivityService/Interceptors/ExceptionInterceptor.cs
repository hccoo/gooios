﻿using AspectCore.DynamicProxy;
using Gooios.Infrastructure.Exceptions;
using System;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Interceptors
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
