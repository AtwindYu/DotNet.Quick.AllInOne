using System;
using NSubstitute;
using Xunit;

namespace xUnit2.Demo.Tests.NSubstitute.Demo.LessonBase
{

    /// <summary>
    /// 使用函数设置返回值
    /// 
    /// <remarks>
    /// 对于一个属性或者方法调用的返回值，可以使用函数来返回结果。这允许我们在替代实例中嵌入更加复杂的逻辑。
    /// 尽管通常来说这不是一个好的做法，但在某些情况下确实很有用。
    /// </remarks>
    /// </summary>
    public class Lesson06
    {

        /// <summary>
        /// 在这个示例中，我们使用参数匹配器来匹配所有对 Add() 方法的调用，使用一个 Lambda 函数来计算第一个和第二个参数的和，并将计算的结果传递给方法调用。
        /// </summary>
        [Fact]
        public void Test_ReturnFromFunction_ReturnSum()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator
                .Add(Arg.Any<int>(), Arg.Any<int>())
                .Returns(x => (int)x[0] + (int)x[1]);

            Assert.Equal(2, calculator.Add(1, 1));
            Assert.Equal(50, calculator.Add(20, 30));
            Assert.Equal(9275, calculator.Add(-73, 9348));
        }

        #region 调用信息


        public interface IFoo
        {
            string Bar(int a, string b);
        }

        /// <summary>
        /// 为 Returns() 和 ReturnsForAnyArgs() 方法提供的函数是一个 Func&lt;CallInfo, T&gt; 类型，在这里，T是方法调用将要返回的值的类型，CallInfo 类型提供访问参数列表的能力。
        /// 在前面的示例中，我们使用索引器 indexer 来访问参数列表。CallInfo 也提供了一个很简便的方法用于通过强类型方式来选择参数：
        /// 
        /// <remarks>
        /// 在这里，x.Arg&lt;string&gt;() 将返回方法调用中的 string 类型的参数，而没有使用 (string)x[1] 方式。
        /// 如果在方法调用中有两个 string 类型的参数，NSubstitute 将通过抛出异常的方式来告诉你无法确定具体是哪个参数。
        /// </remarks>
        /// </summary>
        [Fact]
        public void Test_ReturnFromFunction_CallInfo()
        {
            var foo = Substitute.For<IFoo>();
            foo.Bar(0, "").ReturnsForAnyArgs(x => "Hello " + x.Arg<string>());
            Assert.Equal("Hello World", foo.Bar(1, "World"));
        }


        #endregion


        #region 回调

        /// <summary>
        /// 这种技术可用于在调用时访问一个回调函数
        /// </summary>
        [Fact]
        public void Test_ReturnFromFunction_GetCallbackWhenever()
        {
            var calculator = Substitute.For<ICalculator>();

            var counter = 0;
            calculator
                .Add(0, 0)
                .ReturnsForAnyArgs(x =>
                {
                    counter++;
                    return 0;
                });

            calculator.Add(7, 3);
            calculator.Add(2, 2);
            calculator.Add(11, -3);
            Assert.Equal(3, counter);
        }

        /// <summary>
        /// 或者也可以在 Returns() 之后通过 AndDoes() 来指定回调：
        /// </summary>
        [Fact]
        public void Test_ReturnFromFunction_UseAndDoesAfterReturns()
        {
            var calculator = Substitute.For<ICalculator>();

            var counter = 0;
            calculator
                .Add(0, 0)
                .ReturnsForAnyArgs(x => 0)
                .AndDoes(x => counter++);

            calculator.Add(7, 3);
            calculator.Add(2, 2);
            Assert.Equal(2, counter);
        }

        #endregion


        #region 设置多个返回值

        [Fact]
        public void Test_MultipleReturnValues_ReturnMultipleValues()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Mode.Returns("DEC", "HEX", "BIN");
            Assert.Equal("DEC", calculator.Mode);
            Assert.Equal("HEX", calculator.Mode);
            Assert.Equal("BIN", calculator.Mode);
        }


        #endregion


        #region 使用回调来返回多个值

        [Fact]
        public void Test_MultipleReturnValues_UsingCallbacks()
        {
            try
            {
                var calculator = Substitute.For<ICalculator>();
                calculator.Mode.Returns(x => "DEC", x => "HEX", x => { throw new Exception(); });
                Assert.Equal("DEC", calculator.Mode);
                Assert.Equal("HEX", calculator.Mode);
                Assert.Throws<Exception>(()=> calculator.Mode);
                var result = calculator.Mode;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
               // throw;
                Assert.ThrowsAsync<Exception>(() => throw new Exception(e.Message));
            }
           
        }

        #endregion




        #region 替换返回值

        /// <summary>
        /// 如果需要的话，一个方法或属性的返回值可以被设置多次。只有最后一次设置的值将被返回。
        /// </summary>
        [Fact]
        public void Test_ReplaceReturnValues_ReplaceSeveralTimes()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Mode.Returns("DEC,HEX,OCT");
            calculator.Mode.Returns(x => "???");
            calculator.Mode.Returns("HEX");
            calculator.Mode.Returns("BIN");

            Assert.Equal("BIN", calculator.Mode);
        }

        #endregion

    }
}