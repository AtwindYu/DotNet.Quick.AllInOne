using System.Threading;
using Xunit;

namespace xUnit2.Demo.Tests.UnitTestParallelism
{


    /*
     How does xUnit.net decide which tests can run against each other in parallel? It uses a concept called test collections to make that decision.

By default, each test class is a unique test collection. Tests within the same test class will not run in parallel against each other. Let's examine a very simple test assembly, one with a single test class:

     
     */

    /// <summary>
    /// 同一个Class下的测试，将不会并行测试。同一类中的测试是同一个测试集合，同一个测试集合不会并行测试。
    /// </summary>
    public class TestClass1
    {
        [Fact]
        public void Test1()
        {
            Thread.Sleep(3000);
        }

        [Fact]
        public void Test2()
        {
            Thread.Sleep(5000);
        }
    }


    /*
     When we run this test assembly, we see that the total time spent running the tests is approximately 8 seconds. These two tests are in the same test class, which means that they are in the same test collection, so they cannot be run in parallel against one another.

If we were to put these two tests into separate test classes, like this:
     */

  

    #region 下面的 TestClass2 将与 TestClass1 同时开始测试。因为它们是处于不同的测试集合（Test Collections）中

    public class TestClass12
    {
        [Fact]
        public void Test1()
        {
            Thread.Sleep(3000);
        }
    }


    public class TestClass22
    {
        [Fact]
        public void Test2()
        {
            Thread.Sleep(5000);
        }
    }

    #endregion

    /*
     Now when we run this test assembly, we see that the total time spent running the tests is approximately 5 seconds. That's because Test1 and Test2 are in different test collections, so they are able to run in parallel against one another.
     */

    #region 命名了同一测试集合，那么它们将不能并行运行

    [Collection("Our Test Collection #1")]
    public class TestClass11
    {
        [Fact]
        public void Test1()
        {
            Thread.Sleep(3000);
        }
    }

    [Collection("Our Test Collection #1")]
    public class TestClass21
    {
        [Fact]
        public void Test2()
        {
            Thread.Sleep(5000);
        }
    }

    #endregion

}