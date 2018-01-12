using System;
using System.Collections.Generic;
using NSubstitute;
using Xunit;

namespace xUnit2.Demo.Tests.NSubstitute.Demo.LessonBase
{
    //http://www.cnblogs.com/gaochundong/archive/2013/05/22/nsubstitute_checking_received_calls.html

    /// <summary>
    /// 检查接收到的调用
    /// </summary>
    public class Lesson09
    {


        public interface ICommand
        {
            void Execute();
            event EventHandler Executed;
        }

        public class SomethingThatNeedsACommand
        {
            ICommand command;
            public SomethingThatNeedsACommand(ICommand command)
            {
                this.command = command;
            }
            public void DoSomething() {
                //command.Execute(); //如果注掉就会异常，提示没有调用ICommand里的Exceucte方法
                command.Execute();
            }
            public void DontDoAnything() { }
        }

        /// <summary>
        /// 在某些情况下（尤其是对void方法），检测替代实例是否能成功接收到一个特定的调用是非常有用的。可以通过使用 Received() 扩展方法，并紧跟着被检测的方法。
        /// 
        /// <remarks>
        /// 在这个例子中，command 收到了一个对 Execute() 方法的调用，所以会顺利地完成。
        /// 如果没有收到对 Execute() 的调用，则 NSubstitute 会抛出一个 ReceivedCallsException 异常，并且会显示具体是在期待什么方法被调用，参数是什么，以及列出实际的方法调用和参数。
        /// </remarks>
        /// </summary>
        [Fact]
        public void Test_CheckReceivedCalls_CallReceived()
        {
            //Arrange
            var command = Substitute.For<ICommand>();
            var something = new SomethingThatNeedsACommand(command);

            //Act
            something.DoSomething();

            //Assert
            command.Received().Execute();
        }


        #region 检查一个调用没有被收到


        /// <summary>
        /// 通过使用 DidNotReceive() 扩展方法，NSubstitute 可以确定一个调用未被接收到。
        /// </summary>
        [Fact]
        public void Test_CheckReceivedCalls_CallDidNotReceived()
        {
            //Arrange
            var command = Substitute.For<ICommand>();
            var something = new SomethingThatNeedsACommand(command);

            //Act
            something.DontDoAnything();

            //Assert
            command.DidNotReceive().Execute(); //DontDoAnything 里没有调用command的Excute方法
        }


        #endregion


        #region 检查接收到指定次数的调用



        public class CommandRepeater
        {
            ICommand command;
            int numberOfTimesToCall;
            public CommandRepeater(ICommand command, int numberOfTimesToCall)
            {
                this.command = command;
                this.numberOfTimesToCall = numberOfTimesToCall;
            }

            public void Execute()
            {
                for (var i = 0; i < numberOfTimesToCall; i++) command.Execute();
            }
        }

        /// <summary>
        /// Received() 扩展方法会对某成员的至少一次的调用进行断言，DidNotReceive() 则会断言未收到调用。
        /// NSubstitute 也提供允许断言某调用是否接收到了指定的次数的选择，通过传递一个整型值给 Received() 方法。
        /// 如果替代实例没有接收到给定的次数，则将会抛出异常。接收到的次数少于或多于给定次数，断言会失败。
        /// </summary>
        [Fact]
        public void Test_CheckReceivedCalls_CallReceivedNumberOfSpecifiedTimes()
        {
            // Arrange
            var command = Substitute.For<ICommand>();
            var repeater = new CommandRepeater(command, 3);

            // Act
            repeater.Execute();

            // Assert
            // 如果仅接收到2次或者4次，这里会失败。
            command.Received(3).Execute();

            command.Received(0).Execute(); //这种其实就是 command.DidNotReceive().Execute(); 默认是1次：command.Received(1).Execute();

            //Received(1) 会检查该调用收到并且仅收到一次。这与默认的 Received() 不同，其检查该调用至少接收到了一次。Received(0) 的行为与 DidNotReceive() 相同。
        }


        #endregion


        #region 接收到或者未接收到指定的参数

        public interface ICalculator
        {
            int Add(int a, int b);
            string Mode { get; set; }
        }

        /// <summary>
        /// 我们也可以使用参数匹配器来检查是否收到了或者未收到包含特定参数的调用。
        /// 在参数匹配器一节会介绍更多细节，下面的例子演示了一般用法：
        /// </summary>
        [Fact]
        public void Test_CheckReceivedCalls_CallReceivedWithSpecificArguments()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Add(1, 2);
            calculator.Add(-100, 100);

            // 检查接收到了第一个参数为任意值，第二个参数为2的调用
            calculator.Received().Add(Arg.Any<int>(), 2);
            // 检查接收到了第一个参数小于0，第二个参数为100的调用
            calculator.Received().Add(Arg.Is<int>(x => x < 0), 100);
            // 检查未接收到第一个参数为任意值，第二个参数大于等于500的调用
            calculator
                .DidNotReceive()
                .Add(Arg.Any<int>(), Arg.Is<int>(x => x >= 500));
        }


        #endregion


        #region 忽略参数

        /// <summary>
        /// 就像我们可以为任意参数设置返回值一样，NSubstitute 可以检查收到或者未收到调用，同时忽略其中包含的参数。此时我们需要使用 ReceivedWithAnyArgs() 和 DidNotReceiveWithAnyArgs()。
        /// </summary>
        [Fact]
        public void Test_CheckReceivedCalls_IgnoringArguments()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Add(1, 3);

            calculator.ReceivedWithAnyArgs().Add(1, 1);
            calculator.DidNotReceiveWithAnyArgs(); // .Subtract(0, 0);
        }



        #endregion



        #region 检查对属性的调用

        /// <summary>
        /// 同样的语法可以用于检查属性的调用。
        /// 通常情况下，或许我们会避免这种检测，可能我们对所需的行为进行测试更感兴趣，而不是实现细节的精确性（例如，我们可以设置一个属性返回一个值，然后检测该值是否被合理的使用，而不是断言该属性的 getter 被调用了）。
        /// 当然，有些时候检查 getter 和 setter 是否被调用仍然会派的上用场，所以，这里会介绍如何使用该功能：
        /// </summary>
        [Fact]
        public void Test_CheckReceivedCalls_CheckingCallsToPropeties()
        {
            var calculator = Substitute.For<ICalculator>();

            var mode = calculator.Mode;
            calculator.Mode = "TEST";

            // 检查接收到了对属性 getter 的调用
            // 这里需要使用临时变量以通过编译
            var temp = calculator.Received().Mode;

            // 检查接收到了对属性 setter 的调用，参数为"TEST"
            calculator.Received().Mode = "TEST";
        }


        #endregion


        #region 检查调用索引器

        /// <summary>
        /// 其实索引器只是另外一个属性，所以我们可以使用相同的语法来检查索引器调用。
        /// </summary>
        [Fact]
        public void Test_CheckReceivedCalls_CheckingCallsToIndexers()
        {
            var dictionary = Substitute.For<IDictionary<string, int>>();
            dictionary["test"] = 1;

            dictionary.Received()["test"] = 1;
            dictionary.Received()["test"] = Arg.Is<int>(x => x < 5);
        }


        #endregion


        #region 检查事件订阅


        public class CommandWatcher
        {
            ICommand command;
            public CommandWatcher(ICommand command)
            {
                this.command = command;
                this.command.Executed += OnExecuted;
            }
            public bool DidStuff { get; private set; }
            public void OnExecuted(object o, EventArgs e)
            {
                DidStuff = true;
            }
        }

        /// <summary>
        /// 与属性一样，我们通常更赞成测试所需的行为，而非检查对特定事件的订阅。可以通过使用在替代实例上引发一个事件的方式，并且断言我们的类在响应中执行的正确的行为：
        /// </summary>
        [Fact]
        public void Test_CheckReceivedCalls_CheckingEventSubscriptions()
        {
            var command = Substitute.For<ICommand>();
            var watcher = new CommandWatcher(command);

            command.Executed += Raise.Event();

            Assert.True(watcher.DidStuff);
        }

        /// <summary>
        /// 当然，如果需要的话，Received() 会帮助我们断言订阅是否被收到：
        /// </summary>
        [Fact]
        public void Test_CheckReceivedCalls_MakeSureWatcherSubscribesToCommandExecuted()
        {
            var command = Substitute.For<ICommand>();
            var watcher = new CommandWatcher(command);

            // 不推荐这种方法。
            // 更好的办法是测试行为而不是具体实现。
            command.Received().Executed += watcher.OnExecuted;
            // 或者有可能事件处理器是不可访问的。
            command.Received().Executed += Arg.Any<EventHandler>();
        }


        #endregion



    }
}