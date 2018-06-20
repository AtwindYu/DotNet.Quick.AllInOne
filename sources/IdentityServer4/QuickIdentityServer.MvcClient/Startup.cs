using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace QuickIdentityServer.MvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);



            //2. 配置 OpenID Connect 认证

            //services.AddMvc();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    //使用Cookie作为验证用户的主要方法（通过"Cookies"作为 DefaultScheme）。
                    options.DefaultScheme = "Cookies";
                    //DefaultChallengeScheme 设置为"oidc"(OIDC是OpenID Connect的简称)，因为当我们需要用户登录时，我们将使用OpenID Connect方案。
                    options.DefaultChallengeScheme = "oidc";
                })
                //然后我们使用AddCookie添加可以处理Cookie的处理程序。
                .AddCookie("Cookies")
                //AddOpenIdConnect用于配置执行OpenID Connect协议的处理程序。
                .AddOpenIdConnect("oidc", options =>
                {
                    //SignInScheme 用于在OpenID Connect协议完成后使用cookie处理程序发出cookie。
                    options.SignInScheme = "Cookies";

                    //Authority表示id4服务的地址。
                    options.Authority = App.IdentityHost;
                    options.RequireHttpsMetadata = false;

                    //然后我们通过ClientId识别该客户端。
                    options.ClientId = "mvc";
                    //SaveTokens用于在Cookie中保存IdentityServer中的令牌（稍后将需要）。
                    options.SaveTokens = true;
                });


            //在开发过程中，您有时可能会看到一个异常，说明令牌无法验证。
            // 这是因为签名密钥信息是即时创建的，并且只保存在内存中。 
            //当客户端和IdentityServer不同步时，会发生此异常。 
            //只需在客户端重复操作，下次元数据已经追上，一切都应该正常工作。

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //然后确保在每个请求上执行认证服务，在Startup中的Configure方法添加UseAuthentication：
            app.UseAuthentication();//验证中间件应该在MVC之前添加。

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
