using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickIdentityServer.MvcClient.Models;

namespace QuickIdentityServer.MvcClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        //最后一步是触发认证。为了进入HomeController，并在其中一个Action上添加特性[Authorize]
        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }


        /// <summary>
        /// 退出授权登录
        /// <remarks>
        /// 最后一步是给MVC客户端添加注销功能。
        /// 使用IdentityServer等身份验证服务，仅清除本地应用程序Cookie是不够的。 此外，您还需要往身份服务器交互，以清除单点登录会话。
        /// 确切的协议步骤在OpenID Connect中间件内实现，只需将以下代码添加到某个控制器即可触发注销.
        /// 
        /// 
        /// 这将清除本地cookie，然后重定向到IdentityServer。 IdentityServer将清除它的cookie，然后给用户一个链接返回到MVC应用程序。
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
