using System.Collections.Generic;
using IdentityServer4.Models;

namespace QuickIdentityServer
{
    public class Config
    {
        //Defining the InMemory Clients
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