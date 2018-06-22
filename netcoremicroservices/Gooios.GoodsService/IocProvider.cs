using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Autofac;

namespace Gooios.GoodsService
{
    public class IocProvider
    {
        /* 替换
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
        */

        public static IContainer Container { get; set; }

        public static void SetContainer(IContainer container)
        {
            Container = container;
        }

        public static T GetService<T>()
        {
            return Container.Resolve<T>();
        }

        public static IEnumerable<T> GetServices<T>()
        {
            return Container.Resolve<IEnumerable<T>>();
        }

    }
}
