using Xunit;
using Xunit.Abstractions;

namespace xUnit2.Demo.Tests.xUnit.Demo
{

    /// <summary>
    /// AppFixture会只执行一次
    /// <remarks>
    /// 对于ClassFixture而言，因为是基于Class级别的数据共享。so... ... xUnit.Net提供了直接用类继承IClassFixture接口并结合构造函数注入的方式优雅的实现了数据共享的功能
    /// </remarks>
    /// </summary>
    public class SharedContextAppFixture : IClassFixture<AppFixture>
    {
        readonly ITestOutputHelper _output;
        readonly AppFixture _fixture;
        static int _count;

        /// <summary>
        /// 两个参数，都是自动注入实例
        /// </summary>
        /// <param name="output"></param>
        /// <param name="fixture"></param>
        public SharedContextAppFixture(ITestOutputHelper output, AppFixture fixture)
        {
            _output = output;
            _fixture = fixture;
            _count++;
        }

        #region Test case

        [Fact(DisplayName = "SharedContext.ClassFixture.Case01")]
        public void TestCase01()
        {
            _output.WriteLine("Execute case 01! Current User:[{0}]-{1}", _fixture.UserId, _fixture.UserName);
            _output.WriteLine("Execute count! Constructor:[{0}] , ClassFixture:[{1}]", _count, AppFixture.ExecuteCount);

            

            //_output.WriteLine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.FullName);
        }

        [Fact(DisplayName = "SharedContext.ClassFixture.Case02")]
        public void TestCase02()
        {
            _output.WriteLine("Execute case 01! Current User:[{0}]-{1}", _fixture.UserId, _fixture.UserName);
            _output.WriteLine("Execute count! Constructor:[{0}] , ClassFixture:[{1}]", _count, AppFixture.ExecuteCount);
        }

        #endregion Test case


    }
}