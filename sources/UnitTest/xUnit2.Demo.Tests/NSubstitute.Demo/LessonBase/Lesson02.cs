using System;
using NSubstitute;
using Xunit;

namespace xUnit2.Demo.Tests.NSubstitute.Demo.LessonBase
{

    /// <summary>
    /// 
    /// <remarks>
    /// 不要替代类。只替代接口或虚方法
    /// </remarks>
    /// </summary>
    public class Lesson02
    {





        /// <summary>
        /// 有些时候，你可能需要为多个类型创建替代实例。
        /// 一个最好的例子就是，当你有代码使用了某类型后，需要检查是否其实现了 IDisposable 接口，并且确认是否调用了 Dispose 进行类型销毁。
        /// </summary>
        [Fact]
        public void Test_CreatingSubstitute_MultipleInterfaces()
        {
            var command = Substitute.For<ICommand, IDisposable>();

            var runner = new CommandRunner(command);
            runner.RunCommand();

            command.Received().Execute();
            ((IDisposable)command).Received().Dispose();
        }

        /*
         通过这种方法，替代实例可以实现多个类型。
         但请记住，一个类最多只能实现一个类。
         如果你愿意的话，你可以指定多个接口，但是其中只能有一个是类类型。
         为多个类型创建替代实例的最灵活的方式是使用重载。
         */

        [Fact]
        public void Test_CreatingSubstitute_SpecifiedOneClassType()
        {
            var substitute = Substitute.For(
                new[] { typeof(ICommand), typeof(IDisposable), typeof(SomeClassWithCtorArgs) },
                new object[] { 5, "hello world" }
            );
            //Assert.IsType<ICommand>(substitute);
            //Assert.IsType<IDisposable>(substitute);
            Assert.IsType<SomeClassWithCtorArgs>(substitute); //失败，这个是代理对像
        }

        /// <summary>
        /// 通过使用 Substiute.For<T>() 语法，NSubstitute 可以为委托类型创建替代。
        /// 当为委托类型创建替代时，将无法使该替代实例实现额外的接口或类。
        /// </summary>
        [Fact]
        public void Test_CreatingSubstitute_ForDelegate()
        {
            var func = Substitute.For<Func<string>>();
            func().Returns("hello");
            Assert.Equal<string>("hello", func());
        }

    }

    public class SomeClassWithCtorArgs : IDisposable
    {
        public SomeClassWithCtorArgs(int arg1, string arg2)
        {
        }

        public void Dispose() { }
    }

    public interface ICommand : IDisposable
    {
        void Execute();
    }

    public class CommandRunner
    {
        private ICommand _command;

        public CommandRunner(ICommand command)
        {
            _command = command;
        }

        public void RunCommand()
        {
            _command.Execute();
            _command.Dispose();
        }
    }

   
}