using System;
using Microsoft.Extensions.DependencyInjection;

namespace Gooios.AuthorizationService.Core
{
    public class IocProvider
    {
        public static IServiceProvider Container { get; set; }

        public static void SetContainer(IServiceProvider container)
        {
            Container = container;
        }

        public static T CreateType<T>()
        {
            return Container.GetService<T>();
        }
    }
}
