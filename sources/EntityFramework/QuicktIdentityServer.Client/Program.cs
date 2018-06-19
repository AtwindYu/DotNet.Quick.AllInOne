using System;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace QuicktIdentityServer.Client
{
    class Program
    {


        static void Main(string[] args)
        {
            Run().Wait();



            //Console.WriteLine("Hello World!");
            Console.Read();
        }


        static async Task Run()
        {
            //IdentityModel 包含了一个用于 发现端点 的客户端库。这样一来你只需要知道 IdentityServer 的基础地址，实际的端点地址可以从元数据中读取：
            // 从元数据中发现端口
            var disco = await DiscoveryClient.GetAsync("http://localhost:55554");


            //接着你可以使用 TokenClient 来请求令牌。为了创建一个该类型的实例，你需要传入令牌端点地址、客户端id和密码。
            //然后你可以使用 RequestClientCredentialsAsync 方法来为你的目标 API 请求一个令牌：
            // 请求令牌
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine("ERROR:" + tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            /*
             {
  "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImJiYTUyOTJkNzg2YTY2Y2JiZTlhNjU2OGQ0OTA5ZDZmIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MjkzODE4NzcsImV4cCI6MTUyOTM4NTQ3NywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1NTU1NCIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjU1NTU0L3Jlc291cmNlcyIsImFwaTEiXSwiY2xpZW50X2lkIjoiY2xpZW50Iiwic2NvcGUiOlsiYXBpMSJdfQ.AKUfEntz4e3VMOtFxQR0NPkKsx233aS2CXgGSCFEAcGAF5I6XVvoahSqQf7zOwPO8rL4E56RpMd0rA9ViyRSRPzFX3z1tFCH-iPi3myNuAPGB4IscW8UER0q_47E8EpU4gznTScvToznzd6O4Jt2DcDEBfS3gVlfO6TwyKAynY2e97eo3AycURjm7U4KsmuFKJSfP6jWTr_FRA-IrUvhXnQNU-_DacAsytBsFJwHGzVqe6IXTKcdmJ8CcEOXJBimCpBQIQf9SiqinkIkCXaGAO6EaqrkD2nUlon0mScSha71wm1ui_FDeH0iQOWp6jaL4gezHrP5GRIQ5OCwsuZ1CA",
  "expires_in": 3600,
  "token_type": "Bearer"
}



             
             */


        }

    }
}
