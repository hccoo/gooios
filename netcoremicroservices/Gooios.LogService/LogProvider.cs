using Gooios.LogService.Models;
using Gooios.LogService.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gooios.LogService
{
    public class LogProvider
    {
        public static ConcurrentQueue<Log> LogQueue { get; set; }

        static LogProvider()
        {
            LogQueue = new ConcurrentQueue<Log>();
        }

        public static void EnQueue(Log log)
        {
            LogQueue.Enqueue(log);
        }

        public static Log DeQueue()
        {
            Log log;
            LogQueue.TryDequeue(out log);
            return log;
        }

        public static void Start()
        {
            ThreadPool.QueueUserWorkItem(Process);
            ThreadPool.QueueUserWorkItem(Process);
            ThreadPool.QueueUserWorkItem(Process);

            //var worker = new Thread(new ThreadStart(Process));
            //worker.Start();

        }

        public static void Process(object state)
        {
            DatabaseContext dbContext = IocProvider.GetService<DatabaseContext>();
            try
            {
                while (true)
                {
                    try
                    {
                        var log = DeQueue();

                        if (log != null)
                        {
                            dbContext.SystemLogs.Add(new Entities.SystemLog
                            {
                                ApplicationKey = log.ApplicationKey,
                                AppServiceName = log.AppServiceName,
                                BizData = log.BizData,
                                CallerApplicationKey = log.CallerApplicationKey,
                                CreatedOn = DateTime.Now,
                                Exception = log.Exception,
                                Level = log.Level,
                                LogThread = log.LogThread,
                                LogTime = log.LogTime,
                                Operation = log.Operation,
                                ReturnValue = log.ReturnValue
                            });
                            dbContext.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    Thread.Sleep(50);
                }
            }
            finally
            {
                dbContext.Dispose();
            }
        }
    }
}
