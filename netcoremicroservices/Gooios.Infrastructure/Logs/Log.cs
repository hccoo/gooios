using System;
using System.Collections.Generic;
using System.Text;

namespace Gooios.Infrastructure.Logs
{
    public class Log
    {
        public DateTime LogTime { get; set; }

        public int LogThread { get; set; }

        public LogLevel Level { get; set; }

        public string Exception { get; set; }

        public string ApplicationKey { get; set; }

        public string CallerApplicationKey { get; set; }

        public string AppServiceName { get; set; }

        public string Operation { get; set; }

        public string BizData { get; set; }

        public string ReturnValue { get; set; }
    }

    public enum LogLevel
    {
        INFO = 1,
        ERROR = 4
    }
}
