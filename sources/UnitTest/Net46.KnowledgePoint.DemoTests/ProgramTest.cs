using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RiseSoho.Service.Core;

namespace MsTestv2.Demo.Tests
{

    /// <summary>
    /// 目前看来只有MSTest能很好的支持测试资源管理器和LiveTest。
    /// </summary>
    [TestClass()]
    public class ProgramTest
    {
        [TestMethod()]
        public void DemoTest()
        {
            Assert.IsTrue(new App().Add(1, 1) == 2);
        }

       
    }
}