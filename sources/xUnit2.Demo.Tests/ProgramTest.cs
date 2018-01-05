using RiseSoho.Service.Core;
using Xunit;

namespace xUnit2.Demo.Tests
{

    /********************************************************************************************************************
    **
    **        xUnit.Net学习地址：http://www.cnblogs.com/NorthAlan/category/786472.html
    **
    ************************************************************************************************************************/


    /// <summary>
    /// 
    /// </summary>
    public class ProgramTest
    {
        [Fact()]
        public void TestDemo()
        {
            //Assert.True(false, "This test needs an implementation");
            Assert.True(new App().Add(1, 1) == 2);
        }

       
    }
}