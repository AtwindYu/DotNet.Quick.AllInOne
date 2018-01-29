using System;
using NSubstitute;
using Xunit;

namespace xUnit2.Demo.Tests.NSubstitute.Demo.LessonBase
{

    /// <summary>
    /// 检查调用顺序
    /// </summary>
    public class Lesson18
    {
        /*
         * 注：目前这个功能还在 NSubstitute.Experimental 名空间下，我们还在对其 API 和行为进行试验。非常欢迎你来试一试，但是要注意其可能在后续的版本中有变化。请在讨论组中反馈意见。
         * 
         * 有时调用需要满足特定的顺序。就像已知的 "Temporal Coupling"，其取决于调用收到的时间。理想情况下，我们可能会修改设计来移除这些耦合。但当不能移除时，凭借 NSubstitute 我们可以断言调用的顺序。
         */


        public class Controller
        {
            private IConnection connection;
            private ICommand command;
            public Controller(IConnection connection, ICommand command)
            {
                this.connection = connection;
                this.command = command;
            }

            public void DoStuff()
            {
                connection.Open();
                command.Run(connection);
                connection.Close();
            }
        }

        public class ICommand
        {
            public void Run(IConnection connection) { }
        }

        public class IConnection
        {
            public void Open() { }

            public void Close() { }

            public event Action SomethingHappened;
        }

        [Fact]
        public void Test_CheckingCallOrder_CommandRunWhileConnectionIsOpen()
        {
            var connection = Substitute.For<IConnection>();
            var command = Substitute.For<ICommand>();
            var subject = new Controller(connection, command);

            subject.DoStuff();

            Received.InOrder(() =>
            {
                connection.Open();
                command.Run(connection);
                connection.Close();
            });
        }


        //如果接收到调用的顺序不同，Received.InOrder 会抛出异常，并显示期待的结果和实际的调用结果。
        // 我们也可以使用标准的参数匹配器来匹配调用，就像当我们需要检查单个调用时一样。

        [Fact]
        public void Test_CheckingCallOrder_SubscribeToEventBeforeOpeningConnection()
        {
            var connection = Substitute.For<IConnection>();
            connection.SomethingHappened += () => { /* some event handler */ };
            connection.Open();

            Received.InOrder(() =>
            {
                connection.SomethingHappened += Arg.Any<Action>();
                connection.Open();
            });
        }

    }
}