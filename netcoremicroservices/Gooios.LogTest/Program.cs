using Gooios.Infrastructure.Logs;
using System;

namespace Gooios.LogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Guid.NewGuid().GetHashCode());
            Console.WriteLine((DateTime.Now.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
            Console.WriteLine($"{DateTime.Now.ToString("yyMMdd")}{Guid.NewGuid().GetHashCode().ToString()}");
            Console.ReadLine();
        }

        static void Test()
        {
            var i = 0;
            while (i < 10000)
            {
                LogProvider.Trace(new Log
                {
                    ApplicationKey = "sddsddsee232324234",
                    AppServiceName = "LogTest",
                    BizData = "{...}",
                    CallerApplicationKey = "223344",
                    Exception = "exception content",
                    Level = LogLevel.INFO,
                    LogThread = 12,
                    LogTime = DateTime.Now,
                    Operation = "test",
                    ReturnValue = "{..;;;;}"
                });
                i++;
                Console.WriteLine(i);
            }
        }
    }
}
