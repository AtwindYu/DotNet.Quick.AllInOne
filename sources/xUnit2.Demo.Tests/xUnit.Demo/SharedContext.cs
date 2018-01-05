using System;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace xUnit2.Demo.Tests.xUnit.Demo
{

    /// <summary>
    /// 构造方法和Dispose会在每个方法调用前后都执行一次（注意了，会自动加到每个方法的前后来执行，不是只执行一次）。
    /// </summary>
    public class SharedContext : IDisposable
    {

        private readonly ITestOutputHelper _output;

        /// <summary>
        /// 初始化
        /// </summary>
        public SharedContext(ITestOutputHelper output)
        {
            Thread.Sleep(50);
            _output = output;
            _output.WriteLine($"[{DateTime.Now:HH:mm:ss ffffff}] Execute SharedContext constructor!");
        }

        #region Test case
        [Fact(DisplayName = "SharedContext.Constructor.Case01")]
        public void TestCase01()
        {
            Thread.Sleep(50);
            _output.WriteLine($"[{DateTime.Now:HH:mm:ss ffffff}] Execute case 01!");
        }

        [Fact(DisplayName = "SharedContext.Constructor.Case02")]
        public void TestCase02()
        {
            Thread.Sleep(50);
            _output.WriteLine($"[{DateTime.Now:HH:mm:ss ffffff}] Execute case 02!");
        }

        [Fact(DisplayName = "SharedContext.Constructor.Case03")]
        public void TestCase03()
        {
            Thread.Sleep(50);
            _output.WriteLine($"[{DateTime.Now:HH:mm:ss ffffff}] Execute case 03!");
        }
        #endregion


        /// <summary>
        /// 清理
        /// </summary>
        public void Dispose()
        {
            Thread.Sleep(50);
            _output.WriteLine($"[{DateTime.Now:HH:mm:ss ffffff}] Execute dispose!");
        }
    }
}