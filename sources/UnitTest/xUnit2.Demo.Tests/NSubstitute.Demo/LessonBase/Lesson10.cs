using NSubstitute;
using Xunit;

namespace xUnit2.Demo.Tests.NSubstitute.Demo.LessonBase
{
    //http://www.cnblogs.com/gaochundong/archive/2013/05/22/nsubstitute_clearing_received_calls.html

    /// <summary>
    /// 清理已收到的调用
    /// </summary>
    public class Lesson10
    {

        //通过使用 ClearReceivedCalls() 扩展方法，可以使替代实例忘记先前的所有调用。
        //比如说我们有一个 ICommand 接口，我们想让 OnceOffCommandRunne r接收一个 ICommand 然后仅执行一次。

        public interface ICommand
        {
            void Execute();
        }

        public class OnceOffCommandRunner
        {
            ICommand command;
            public OnceOffCommandRunner(ICommand command)
            {
                this.command = command;
            }
            public void Run()
            {
                if (command == null) return;
                command.Execute();
                command = null;
            }
        }

        //如果我们为 ICommand 创建替代，则我们可以其在第一运行时即被调用，然后忘记之前的所有调用，之后再确定其没有被再次调用。
        [Fact]
        public void Test_ClearReceivedCalls_ForgetPreviousCalls()
        {
            var command = Substitute.For<ICommand>();
            var runner = new OnceOffCommandRunner(command);

            // 第一次运行
            runner.Run();
            command.Received().Execute();

            // 忘记前面对command的调用
            command.ClearReceivedCalls();

            // 第二次运行
            runner.Run();
            command.DidNotReceive().Execute();
        }

        //ClearReceivedCalls() 不会清理通过 Returns() 为替代实例设定的返回值。如果我们需要这么做，则可通过再次调用 Returns() 来替换之前指定的值的方式来进行。
    }
}