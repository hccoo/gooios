using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gooios.VerificationService.Application.DTO;
using Gooios.VerificationService.Application.Services;
using Gooios.VerificationService.Domain.Aggregates;
using Microsoft.Extensions.Primitives;

namespace Gooios.VerificationService.Controllers
{
    [Route("api/[controller]/v1")]
    public class VerificationController : Controller
    {
        readonly IVerificationAppService _verificationAppService;

        public VerificationController(IVerificationAppService verificationAppService)
        {
            _verificationAppService = verificationAppService;
        }

        // GET api/values/5
        [HttpGet("{bizcode}/{to}")]
        public VerificationDTO Get(BizCode bizCode, string to)
        {
            return _verificationAppService.GetAvailableVerification(bizCode, to);
        }

        public void Patch([FromBody]VerificationDTO model)
        {
            _verificationAppService.SetVerificationUsed(model);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]VerificationDTO model)
        {
            _verificationAppService.AddVerification(model);
        }
    }
}
