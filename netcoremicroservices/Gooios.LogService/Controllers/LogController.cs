using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gooios.LogService.Models;
using Gooios.LogService.Repositories;

namespace Gooios.LogService.Controllers
{
    [Route("api/[controller]/v1")]
    public class LogController : Controller
    {
        // POST api/values
        [HttpPost]
        public void Post([FromBody]Log log)
        {
            //LogProvider.EnQueue(value);

            var dbContext = IocProvider.GetService<DatabaseContext>();
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
}
