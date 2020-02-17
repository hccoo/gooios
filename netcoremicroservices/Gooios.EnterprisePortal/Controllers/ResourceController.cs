using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Gooios.EnterprisePortal.Controllers
{
    [Produces("application/json")]
    [Route("api/resource")]
    public class ResourceController : Controller
    {
        public dynamic Get()
        {
            HttpContext.Request.Headers.TryGetValue("apikey", out StringValues vals);

            var apikey = vals.FirstOrDefault();
            if (apikey.ToUpper() != "8E7B6C68946341E38ECCF1F87A0D589B") return null;

            var obj = new {
                Banners = new List<string> {
                    $"https://{this.HttpContext.Request.Host}/images/applet_resources/banner_01.jpg",
                    $"https://{this.HttpContext.Request.Host}/images/applet_resources/banner_02.jpg",
                    $"https://{this.HttpContext.Request.Host}/images/applet_resources/banner_03.jpg"
                },
                ADBanner = $"https://{this.HttpContext.Request.Host}/images/applet_resources/banner_04.jpg"
            };
            return obj;
        }
    }
}