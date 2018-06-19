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
            var users = User.Identities.Select(x => new { x.Name, x.RoleClaimType });

            //return new JsonResult(new { claims = User.Claims.Select(x => new { x.Type, x.Value }) });
            //上面的对象，都不能完整的输出。

            return new JsonResult(User.Claims.Select(x => new { x.Type, x.Value }));
        }
    }
}