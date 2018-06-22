using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Gooios.PartnerGateway
{
    public class IocProvider
    {
        public static IServiceProvider Container { get; set; }

        public static void SetContainer(IServiceProvider container)
        {
            Container = container;
        }

        public static T GetService<T>()
        {
            return Container.GetService<T>();
        }

        public static IEnumerable<T> GetServices<T>()
        {
            return Container.GetServices<T>();
        }
    }
}
