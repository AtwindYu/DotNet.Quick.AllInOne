using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnit2.Demo.Tests.RetryFactExample
{

    /*
     
         上面的代码主要要注意以下几点：

        自定义的TestCase类最好是继承自XunitTestCase（如果有更深层次的要求可以直接实现IXunitTestCase）。
        重写基类的RunAsync方法，该方法会在Runner运行Test Case的时候被调用。
        重写Serialize / Deserialize 方法，像xUnit.Net上下文中添加对自定义属性值的序列化/反序列化的支持。
        目前，无参构造函数RetryTestCase目前是必须有的（后续的版本中应当会移除掉）。否则，Runner会无法构造无参的Case。
         最后，在RunAsync中，我们根据用户设置的次数运行测试用例。如果一直没有成功，则会向消息接收器中添加一个错误的Message（该消息最终会通过消息总线返回给实际的Runner）。可以看到，DelayedMessageBus （代码中 Line38） 是我们自定义的消息总线。


         */


    [Serializable]
    public class RetryTestCase : XunitTestCase
    {
        private int maxRetries;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", true)]
        public RetryTestCase() { }

        public RetryTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay testMethodDisplay, ITestMethod testMethod, int maxRetries)
            : base(diagnosticMessageSink, testMethodDisplay, testMethod, testMethodArguments: null)
        {
            this.maxRetries = maxRetries;
        }

        // This method is called by the xUnit test framework classes to run the test case. We will do the
        // loop here, forwarding on to the implementation in XunitTestCase to do the heavy lifting. We will
        // continue to re-run the test until the aggregator has an error (meaning that some internal error
        // condition happened), or the test runs without failure, or we've hit the maximum number of tries.
        public override async Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                        IMessageBus messageBus,
                                                        object[] constructorArguments,
                                                        ExceptionAggregator aggregator,
                                                        CancellationTokenSource cancellationTokenSource)
        {
            var runCount = 0;

            while (true)
            {
                // This is really the only tricky bit: we need to capture and delay messages (since those will
                // contain run status) until we know we've decided to accept the final result;
                var delayedMessageBus = new DelayedMessageBus(messageBus);

                var summary = await base.RunAsync(diagnosticMessageSink, delayedMessageBus, constructorArguments, aggregator, cancellationTokenSource);
                if (aggregator.HasExceptions || summary.Failed == 0 || ++runCount >= maxRetries)
                {
                    delayedMessageBus.Dispose();  // Sends all the delayed messages
                    return summary;
                }

                diagnosticMessageSink.OnMessage(new DiagnosticMessage("Execution of '{0}' failed (attempt #{1}), retrying...", DisplayName, runCount));
            }
        }

        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);

            data.AddValue("MaxRetries", maxRetries);
        }

        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);

            maxRetries = data.GetValue<int>("MaxRetries");
        }
    }
}
