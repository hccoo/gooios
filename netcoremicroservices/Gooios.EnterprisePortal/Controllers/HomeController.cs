using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gooios.EnterprisePortal.Models;
using Gooios.Infrastructure.Logs;

namespace Gooios.EnterprisePortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Products()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult WeChatNotify([FromBody]object obj)
        {
            var logTask = LogProvider.Trace(new Log
            {
                ApplicationKey = "968960bff18111e799160126c7e9f008",
                AppServiceName = "EnterprisePortal",
                BizData = "test123",
                CallerApplicationKey = "wechat",
                Exception = "",
                Level = LogLevel.INFO,
                LogTime = DateTime.Now,
                Operation = "paymentnotify",
                ReturnValue = ""
            });

            return Ok("<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>");
        }
    }
}
