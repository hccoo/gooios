using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.LogService.Entities
{
    public class SystemLog
    {
        public int Id { get; set; }

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

        public DateTime CreatedOn { get; set; }
    }

    public enum LogLevel
    {
        INFO=1,
        ERROR=4
    }
}
