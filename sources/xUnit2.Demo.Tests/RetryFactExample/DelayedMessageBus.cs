using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnit2.Demo.Tests.RetryFactExample
{
    /*
     与Runner交流：消息总线 - IMessageBus

        在测试用例被xUnit.Net对应的Runner运行的时候，Runner和测试框架的消息沟通是通过消息总线的形式来实现的，这也是很多类似系统都会提供的能力。
        IMessageBus中定义了向运行xUnit.Net测试用的Runner发送消息的接口方法QueueMessage：
         
         */

    /// <summary>
    /// Used to capture messages to potentially be forwarded later. 
    /// Messages are forwarded by disposing of the message bus.
    /// </summary>
    public class DelayedMessageBus : IMessageBus
    {
        private readonly IMessageBus innerBus;
        private readonly List<IMessageSinkMessage> messages = new List<IMessageSinkMessage>();

        public DelayedMessageBus(IMessageBus innerBus)
        {
            this.innerBus = innerBus;
        }

        public bool QueueMessage(IMessageSinkMessage message)
        {
            lock (messages)
                messages.Add(message);

            // No way to ask the inner bus if they want to cancel without sending them the message, so
            // we just go ahead and continue always.
            return true;
        }

        public void Dispose()
        {
            foreach (var message in messages)
                innerBus.QueueMessage(message);
        }
    }
}
