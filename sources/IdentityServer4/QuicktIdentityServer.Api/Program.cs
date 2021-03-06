﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace QuicktIdentityServer.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "ApiServer";

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5010") //如果不明确指定的话，会生成两个URL，一个5000的HTTP和一个5001的HTTPS
                .UseStartup<Startup>();
    }
}
