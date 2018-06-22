using Gooios.LogService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.LogService.Models
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
}
