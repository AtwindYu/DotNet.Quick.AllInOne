using System;
using System.IO;

namespace xUnit2.Demo.Tests.xUnit.Demo
{
    public class AppFixture : IDisposable
    {

        public int UserId { get; set; }
        public string UserName { get; set; }
        public static int ExecuteCount;

     

        public AppFixture()
        {
            this.UserId = 1;
            this.UserName = "Oak";
            ExecuteCount++;
            //ExcelFiles = $"{AppDomain.CurrentDomain.BaseDirectory}\\App_Data\\files\\";

            //全局初始化

        }


        public void Dispose()
        {
            //全局清理
        }
    }
}