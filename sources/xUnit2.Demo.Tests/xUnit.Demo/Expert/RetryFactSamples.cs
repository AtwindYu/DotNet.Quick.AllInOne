using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnit2.Demo.Tests.xUnit.Demo.Expert
{
    /// <summary>
    /// 
    /// <remarks>
    /// 在单元测试的实践中，Fact和Theory已经能满足我们许多的要求。
    /// 但是对于一些特殊的情况，例如：需要多次运行一个方法的测试用例（10秒钟内支付接口只能做3次），或者需要开启多个线程来运行测试用例。
    /// 这些需求我们当然可以通过编码来完成。
    /// 但如果可以用属性标记的方式来简单的实现这样的功能。
    /// 就会大大降低使用者的编程复杂度，这样的能力也是在设计一个单元测试框架的时候需要考虑的。
    /// xUnit.Net为我们提供的优雅的接口，方便我们对框架本身进行扩展。
    /// </remarks>
    /// </summary>
    public class RetryFactSamples
    {
        public class CounterFixture
        {
            public int RunCount;
        }

        public class RetryFactSample : IClassFixture<CounterFixture>
        {
            private readonly CounterFixture counter;

            public RetryFactSample(CounterFixture counter)
            {
                this.counter = counter;
                counter.RunCount++;
            }


            /// <summary>
            /// 可以看到，用来标记测试用了的属性标签不再是xUnit.Net提供的Fact或者Theory了，取而代之的是自定义的RetryFact标签。
            /// 顾名思义，实际的测试过程中标签会按照MaxRetries所设置的次数来重复执行被标记的测试用例。自定义运行标签主要有下面几个步骤：
            /// 1.  创建标签自定义标签
            /// 2. 创建自定义的TestCaseDiscoverer
            /// 3. 创建自定义的XunitTestCase子类
            /// 4. 重写消息总线的传输逻辑
            /// </summary>
            [RetryFact(MaxRetries = 5)]
            public void IWillPassTheSecondTime()
            {
                Assert.Equal(2, counter.RunCount);
            }
        }


    }


    [XunitTestCaseDiscoverer("xUnit2.Demo.Tests.xUnit.Demo.Expert.RetryFactDiscoverer", "xUnit2.Demo.Tests")]
    public class RetryFactAttribute : FactAttribute
    {
        /// <summary>
        /// Number of retries allowed for a failed test. If unset (or set less than 1), will
        /// default to 3 attempts.
        /// </summary>
        public int MaxRetries { get; set; }
    }

    //public class RetryFactDiscoverer : IXunitTestCaseDiscoverer
    //{
    //    readonly IMessageSink diagnosticMessageSink;

    //    public RetryFactDiscoverer(IMessageSink diagnosticMessageSink)
    //    {
    //        this.diagnosticMessageSink = diagnosticMessageSink;
    //    }

    //    public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
    //    {
    //        var maxRetries = factAttribute.GetNamedArgument<int>("MaxRetries");
    //        if (maxRetries < 1)
    //        {
    //            maxRetries = 3;
    //        }
    //       // new RetryTestCase()
           

    //       // yield return new RetryTestCase(diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod, maxRetries);
    //    }
    //}

}