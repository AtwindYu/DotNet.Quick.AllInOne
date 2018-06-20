using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace QuickIdentityServer
{
    public class Startup
    {

        /*
         AddIdentityServer方法在依赖注入系统中注册IdentityServer，它还会注册一个基于内存存储的运行时状态，这对于开发场景非常有用，对于生产场景，您需要一个持久化或共享存储，如数据库或缓存。请查看使用EntityFramework Core实现的存储。

AddDeveloperSigningCredential(1.1为AddTemporarySigningCredential)扩展在每次启动时，为令牌签名创建了一个临时密钥。在生成环境需要一个持久化的密钥。详细请点击
             

            当您切换到self-hosting时，Web服务器端口默认为5000.您可以在上面的启动配置文件对话框中配置，也可以在Program.cs中进行配置，我们在quickstart中为IdentityServer Host使用以下配置：

             */



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //IdentityServer使用通常的模式来配置和添加服务到ASP.NET Core Host
            //
            //在ConfigureServices中，所有必须的服务被配置并且添加到依赖注入系统中。

            //services.AddIdentityServer().AddDeveloperSigningCredential(); //.AddInMemoryClients(InMemoryClientStore);

            //上面的异常，必须改成下面的方式

            //services.AddIdentityServer().AddDeveloperSigningCredential()
            //    .AddInMemoryClients(Config.GetClients());


            ////上面的还是异常，改成下面的
            //services.AddIdentityServer().AddDeveloperSigningCredential()
            //    .AddInMemoryClients(Config.GetClients())
            //    .AddInMemoryApiResources(Config.GetApiResources());


            //            // 使用内存存储，密钥，客户端和资源来配置身份服务器。
            //            services.AddIdentityServer()
            //                .AddDeveloperSigningCredential()
            //                .AddInMemoryApiResources(Config.GetApiResources())//添加api资源
            //                .AddInMemoryClients(Config.GetClients())//添加客户端
            //                .AddTestUsers(Config.GetUsers()); //添加测试用户
            //            /*
            //             AddTestUsers 方法帮我们做了以下几件事：

            //为资源所有者密码授权添加支持
            //添加对用户相关服务的支持，这服务通常为登录 UI 所使用（我们将在下一个快速入门中用到登录 UI）
            //为基于测试用户的身份信息服务添加支持（你将在下一个快速入门中学习更多与之相关的东西）

            //             */





            services.AddMvc();

            //然后，您需要将这些身份资源添加到Startup.cs中的IdentityServer配置中。使用AddInMemoryIdentityResources扩展方法调用AddIdentityServer()：
            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers());


        }




        //在Configure中，中间件被添加到HTTP管道中。
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            loggerFactory.AddConsole(LogLevel.Debug);


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }





            app.UseIdentityServer();

            //app.UseMvc();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }



        
    }
}
