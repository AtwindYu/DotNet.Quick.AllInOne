using System;
using NSubstitute;
using NSubstitute.Exceptions;
using Xunit;

namespace xUnit2.Demo.Tests.NSubstitute.Demo.LessonBase
{

    //http://www.cnblogs.com/gaochundong/archive/2013/05/21/nsubstitute_get_started.html

    public class Lesson01
    {

        /// <summary>
        /// 我们可以让NSubstitute来创建类型实例的替代实例。
        /// 可以创建诸如 Stub、Mock、Fake、Spy、Test Double 等
        /// </summary>
        [Fact]
        public void Test_GetStarted_ReturnSpecifiedValue()
        {
            var calculator = Substitute.For<ICalculator>();
            calculator.Add(1, 2).Returns(3);

            int actual = calculator.Add(1, 2);
            Assert.Equal<int>(3, actual);

            
        }

        /// <summary>
        /// 我们可以检查该替代实例是否接收到了一个指定的调用，或者未收到某指定调用
        /// </summary>
        [Fact]
        public void Test_GetStarted_ReceivedSpecificCall()
        {
            var calculator = Substitute.For<ICalculator>();
            calculator.Add(1, 2);

            calculator.Received().Add(1, 2);
            calculator.DidNotReceive().Add(5, 7);
        }


        /// <summary>
        /// 如果 Received() 断言失败，NSubstitute 会尝试给出有可能是什么问题
        /// </summary>
        [Fact]
        public void Test_GetStarted_DidNotReceivedSpecificCall()
        {
            ICalculator calculator = Substitute.For<ICalculator>();
            calculator.Add(5, 7);

            //calculator.Received().Add(1, 2);//如果 Received() 断言失败，NSubstitute 会尝试给出有可能是什么问题
            //Assert.Throws<ReceivedCallsException>(() => { calculator.Received().Add(1, 2); });
            Assert.Throws<Exception>(() => { calculator.Received().Add(1, 2); });
        }

        /// <summary>
        /// 我们也可以对属性使用与方法类似的 Retures() 语法，或者继续使用简单的属性 setter 来设置返回值。
        /// </summary>
        [Fact]
        public void Test_GetStarted_SetPropertyValue()
        {
            ICalculator calculator = Substitute.For<ICalculator>();

            calculator.Mode.Returns("DEC");
            Assert.Equal<string>("DEC", calculator.Mode);

            calculator.Mode = "HEX";
            Assert.Equal<string>("HEX", calculator.Mode);
        }


        /// <summary>
        /// NSubstitute 支持参数匹配功能，可以设置参数规则，并断言判断是否接收到参数匹配的调用。
        /// </summary>
        [Fact]
        public void Test_GetStarted_MatchArguments()
        {
            ICalculator calculator = Substitute.For<ICalculator>();

            calculator.Add(10, -5);

            calculator.Received().Add(10, Arg.Any<int>());
            calculator.Received().Add(10, Arg.Is<int>(x => x < 0));
        }

        /// <summary>
        /// 我们也可以在使用参数匹配功能的同时，传递一个函数给 Returns() ，以此来使替代实例具有更多的功能。
        /// </summary>
        [Fact]
        public void Test_GetStarted_PassFuncToReturns()
        {
            ICalculator calculator = Substitute.For<ICalculator>();
            calculator
                .Add(Arg.Any<int>(), Arg.Any<int>())
                .Returns(x => (int)x[0] + (int)x[1]);  //传入参数索引

            int actual = calculator.Add(5, 10);

            Assert.Equal<int>(15, actual);
        }


        /// <summary>
        /// Returns() 也可通过构造一个返回值序列来指定多个参数。
        /// </summary>
        [Fact]
        public void Test_GetStarted_MultipleValues()
        {
            ICalculator calculator = Substitute.For<ICalculator>();
            calculator.Mode.Returns("HEX", "DEC", "BIN");

            Assert.Equal<string>("HEX", calculator.Mode);
            Assert.Equal<string>("DEC", calculator.Mode);
            Assert.Equal<string>("BIN", calculator.Mode);
        }


        /// <summary>
        /// 我们可以在替代实例上引发事件通知
        /// </summary>
        [Fact]
        public void Test_GetStarted_RaiseEvents()
        {
            ICalculator calculator = Substitute.For<ICalculator>();
            bool eventWasRaised = false;

            calculator.PoweringUp += (sender, args) =>
            {
                eventWasRaised = true;
            };

            calculator.PoweringUp += Raise.Event();

            Assert.True(eventWasRaised);
        }




    }




    /// <summary>
    /// 一个非常简单的计算器接口
    /// </summary>
    public interface ICalculator
    {
        int Add(int a, int b);
        string Mode { get; set; }

        event EventHandler PoweringUp;
    }


}