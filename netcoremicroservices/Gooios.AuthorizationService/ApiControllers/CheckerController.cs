using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.AuthorizationService.Attributes;

namespace Gooios.AuthorizationService.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Checker/v1")]
    public class CheckerController : Controller
    {
        [ApiKey]
        [HttpGet]
        public string Get()
        {
            return "success";
        }
    }
}