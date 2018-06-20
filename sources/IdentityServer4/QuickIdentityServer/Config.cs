using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer.Core;
using IdentityServer4;
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
                    Username = "joe",
                    Password = "123"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "123",

                    //如上所述，OpenID Connect中间件默认要求配置 profile scope。 这个scope还包括像名字或网站这样的声明。
                    //让我们将这些声明添加到用户，以便IdentityServer可以将它们放入身份令牌中：
                    Claims = new []
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://cszi.com")
                    }
                    //下一次您进行身份验证时，你的声明页面现在将显示额外的声明。
                    //OpenID Connect中间件上的Scope属性是您配置哪些Scope将在身份验证期间发送到IdentityServer。
                    //值得注意的是，对令牌中身份信息的遍历是一个扩展点 - IProfileService。因为我们正在使用 AddTestUser，所以默认使用的是 TestUserProfileService。你可以检出这里的源代码来查看它的工作原理。
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



            //最后一步是将MVC客户端的配置添加到IdentityServer。
            //基于OpenID Connect的客户端与我们迄今添加的OAuth 2.0客户端非常相似。 但是由于OIDC中的流程始终是交互式的，我们需要在配置中添加一些重定向URL。
            //将以下内容添加到您的客户端配置：
            clients.Add(// OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // where to redirect to after login
                    RedirectUris = { $"{App.MvcClientHost}/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { $"{App.MvcClientHost}/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
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





        //# 添加OpenID Connect Identity Scopes的支持
        //与OAuth 2.0类似，OpenID Connect也使用Scopes概念。 
        //再次，Scopes代表您想要保护的客户端希望访问的内容。 
        //与OAuth相反，OIDC中的范围不代表API，而是代表用户ID，姓名或电子邮件地址等身份信息。

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                //所有标准Scopes及其相应的声明都可以在OpenID Connect规范中找到。
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(), 
            };
        }


    }
}