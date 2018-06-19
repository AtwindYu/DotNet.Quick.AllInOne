using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace QuickIdentityServer
{
    public class Config
    {
        //然后将测试用户注册到 IdentityServer：
        public static List<TestUser> GetUsers()
        {
            //TestUser类型表示一个测试用户及其身份信息。
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }


        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            List<Client> clients = new List<Client>();

            //Client1
            clients.Add(new Client()
            {
                ClientId = "Client1",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                }

            });


            clients.Add(new Client
            {
                ClientId = "client",
                // 没有交互性用户，使用 clientid/secret 实现认证。
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // 用于认证的密码
                ClientSecrets =
                 {
                     new Secret("secret".Sha256())
                 },
                // 客户端有权访问的范围（Scopes）
                AllowedScopes = { "api1" }
            });


            //你可以通过修改 ·AllowedGrantTypes· 属性简单地添加对已有客户端授权类型的支持。
            //通常你会想要为资源所有者用例创建独立的客户端，添加以下代码到你配置中的客户端定义中：
            // resource owner password grant client
            clients.Add(new Client
            {
                ClientId = "ro.client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                ClientSecrets =
               {
                   new Secret("secret".Sha256())
               },
                AllowedScopes = { "api1" }
            });





            return clients;
        }

        //Defining the InMemory API's
        public static IEnumerable<ApiResource> GetApiResources()
        {

            List<ApiResource> apiResources = new List<ApiResource>();

            apiResources.Add(new ApiResource("api1", "First API"));

            return apiResources;
        }



    }
}