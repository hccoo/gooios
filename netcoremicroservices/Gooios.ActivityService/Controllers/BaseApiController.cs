using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Gooios.ActivityService.Controllers
{
    public class BaseApiController : Controller
    {
        protected string UserId
        {
            get
            {
                return Request.Headers["userId"].FirstOrDefault();
            }
        }
    }
}