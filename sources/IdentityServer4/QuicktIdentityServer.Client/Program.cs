using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace QuicktIdentityServer.Client
{
    class Program
    {


        static void Main(string[] args)
        {
            //GrantTypeDemo.RequestClientCredentialsAsync().Wait();


            GrantTypeDemo.RequestResourceOwnerPasswordAsync().Wait();



            //Console.WriteLine("Hello World!");
            Console.Read();
        }





    }
}
