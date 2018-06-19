using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuicktIdentityServer.Api.Controllers
{

    [Route("[controller]")]
    [Authorize]
    public class IdentityController : ControllerBase
    {

        // GET
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(User.Claims.Select(x => new { x.Type, x.Value }));
        }
    }
}