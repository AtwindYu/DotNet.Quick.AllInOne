using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer.Core;
using Newtonsoft.Json.Linq;

namespace QuicktIdentityServer.Client
{
    public class GrantTypeDemo
    {

        //public const string IdentityServerHost = "http://localhost:55554";

        //public const string APIUri = "http://localhost:56784/Identity";


        #region API CALL

        static async Task CallApi(string accessToken, string apiName = "/Identity")
        {
            // 调用api
            var client = new HttpClient();
            client.SetBearerToken(accessToken); //设定访问Token

            var response = await client.GetAsync($"{App.ApiHost}{apiName}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("ERROR:" + response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }



        #endregion




        #region 使用客户端认证控制API访问

        /// <summary>
        /// 使用客户端认证控制API访问
        /// </summary>
        /// <returns></returns>
        public static async Task RequestClientCredentialsAsync()
        {
            //IdentityModel 包含了一个用于 发现端点 的客户端库。这样一来你只需要知道 IdentityServer 的基础地址，实际的端点地址可以从元数据中读取：
            // 从元数据中发现端口
            var disco = await DiscoveryClient.GetAsync(App.IdentityHost);


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

            //************ 授权通过了就可以调用API了 *******************

            //最后是调用 API。

            //为了发送访问令牌到 API，你一般要使用 HTTP 授权 header。这可以通过 SetBearerToken 扩展方法来实现：

            await CallApi(tokenResponse.AccessToken);

            //注意：默认情况下访问令牌将包含 scope 身份信息，生命周期（nbf 和 exp），客户端 ID（client_id） 和 发行者名称（iss）。

            /*
[
  {
    "type": "nbf",
    "value": "1529382530"
  },
  {
    "type": "exp",
    "value": "1529386130"
  },
  {
    "type": "iss",
    "value": "http://localhost:55554"
  },
  {
    "type": "aud",
    "value": "http://localhost:55554/resources"
  },
  {
    "type": "aud",
    "value": "api1"
  },
  {
    "type": "client_id",
    "value": "client"
  },
  {
    "type": "scope",
    "value": "api1"
  }
]
             */


        }





        /*


         进一步实践
当前演练目前主要关注的是成功的步骤：

客户端可以请求令牌
客户端可以使用令牌来访问 API



你现在可以尝试引发一些错误来学习系统的相关行为，比如：

尝试在 IdentityServer 未运行时（unavailable）连接它
尝试使用一个非法的客户端id或密码来请求令牌
尝试在请求令牌的过程中请求一个非法的 scope
尝试在 API 未运行时(unavailable)调用它
不向 API 发送令牌   ERROR:Unauthorized
配置 API 为需要不同于令牌中的 scope ERROR:Unauthorized



         */

        #endregion




        #region 使用密码认证方式控制API访问

        /// <summary>
        /// 使用密码认证方式控制API访问
        /// </summary>
        /// <returns></returns>
        public static async Task RequestResourceOwnerPasswordAsync()
        {

            // 从元数据中发现客户端
            var disco = await DiscoveryClient.GetAsync(App.IdentityHost);

            // 请求令牌
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1");//使用用户名密码

            if (tokenResponse.IsError)
            {
                Console.WriteLine("ERROR:" + tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine();

            /*
             
             当你发送令牌到身份 API 端点的时候，你会发现与客户端凭证授权
相比，资源所有者密码授权有一个很小但很重要的区别。访问令牌现在将包含一个 sub 信息，该信息是用户的唯一标识。sub 信息可以在调用 API 后通过检查内容变量来被查看，并且也将被控制台应用程序显示到屏幕上。

sub 信息的存在（或缺失）使得 API 能够区分代表客户端的调用和代表用户的调用。
             
             */

            await CallApi(tokenResponse.AccessToken);
            /*
             * 
             [
  {
    "type": "nbf",
    "value": "1529400621"
  },
  {
    "type": "exp",
    "value": "1529404221"
  },
  {
    "type": "iss",
    "value": "http://localhost:55554"
  },
  {
    "type": "aud",
    "value": "http://localhost:55554/resources"
  },
  {
    "type": "aud",
    "value": "api1"
  },
  {
    "type": "client_id",
    "value": "ro.client"
  },
  {
    "type": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
    "value": "1"
  },
  {
    "type": "auth_time",
    "value": "1529400621"
  },
  {
    "type": "http://schemas.microsoft.com/identity/claims/identityprovider",
    "value": "local"
  },
  {
    "type": "scope",
    "value": "api1"
  },
  {
    "type": "http://schemas.microsoft.com/claims/authnmethodsreferences",
    "value": "pwd"
  }
]
             
             */

        }

        //api资源收到第一个请求之后，会去id4服务器公钥，然后用公钥验证token是否合法，如果合法进行后面后面的有效性验证。
        //有且只有第一个请求才会去id4服务器请求公钥，后面的请求都会用第一次请求的公钥来验证，这也是jwt去中心化验证的思想。

        #endregion








    }
}