using NSubstitute;
using Xunit;

namespace xUnit2.Demo.Tests.NSubstitute.Demo.LessonBase
{

    /// <summary>
    /// 参数匹配器
    /// <remarks>
    /// 参数匹配器可以用于设置返回值和检查接收到的调用。
    /// 它提供了一种指定一个调用或一组调用的方式，这样可以对所有匹配的调用设置一个返回值，或者检查是否接收到了匹配的调用。
    /// </remarks>
    /// </summary>
    public class Lesson11
    {

        #region 忽略参数
        public interface ICalculator
        {
            int Add(int a, int b);
            string Mode { get; set; }
        }

        /// <summary>
        /// 通过使用 Arg.Any&lt;T&gt;() 可以忽略一个T类型的参数。
        /// </summary>
        [Fact]
        public void Test_ArgumentMatchers_IgnoringArguments()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Add(Arg.Any<int>(), 5).Returns(7);

            //在这个例子中，我们设定当任意数与 5 相加时，返回值 7。我们使用 Arg.Any<int>() 来告诉 NSubstitute 忽略第一个参数。
            Assert.Equal(7, calculator.Add(42, 5));
            Assert.Equal(7, calculator.Add(123, 5));
            Assert.Equal(0, calculator.Add(1, 7));
        }



        public interface IFormatter
        {
            void Format(object o);
        }

        //我们也可以通过这种方法来匹配任意的子类型。
        [Fact]
        public void Test_ArgumentMatchers_MatchSubTypes()
        {
            IFormatter formatter = Substitute.For<IFormatter>();

            formatter.Format(new object());
            formatter.Format("some string");

            formatter.Received().Format(Arg.Any<object>());
            formatter.Received().Format(Arg.Any<string>());
            formatter.DidNotReceive().Format(Arg.Any<int>());
        }

        #endregion


        #region 参数条件匹配
        //通过使用 Arg.Is<T>(Predicate<T> condition) 来对一个T类型的参数进行条件匹配。

        [Fact]
        public void Test_ArgumentMatchers_ConditionallyMatching()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Add(1, -10);

            // 检查接收到第一个参数为1，第二个参数小于0的调用
            calculator.Received().Add(1, Arg.Is<int>(x => x < 0));
            // 检查接收到第一个参数为1，第二个参数为 -2、-5和-10中的某个数的调用
            calculator
                .Received()
                //.Add(1, Arg.Is<int>(x => new[] { -2, -5, -10 }.Contains(x)));
                ;
            // 检查未接收到第一个参数大于10，第二个参数为-10的调用
            calculator.DidNotReceive().Add(Arg.Is<int>(x => x > 10), -10);
        }

        //如果某参数的条件表达式抛出异常，则将假设该参数未被匹配，异常本身会被隐藏。

        //[Fact]
        //public void Test_ArgumentMatchers_ConditionallyMatchingThrowException()
        //{
        //    IFormatter formatter = Substitute.For<IFormatter>();

        //    formatter.Format(Arg.Is<string>(x => x.Length <= 10)).Returns("matched");

        //    Assert.Equal("matched", formatter.Format("short"));
        //    Assert.NotEqual("matched", formatter.Format("not matched, too long"));

        //    // 此处将不会匹配，因为在尝试访问 null 的 Length 属性时会抛出异常，
        //    // 而 NSubstitute 会假设其为不匹配并隐藏掉异常。
        //    Assert.NotEqual("matched", formatter.Format(null));
        //}

        #endregion


        #region 匹配指定的参数


        //使用 Arg.Is<T>(T value) 可以匹配指定的T类型参数。

        [Fact]
        public void Test_ArgumentMatchers_MatchingSpecificArgument()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Add(0, 42);

            // 这里可能不工作，NSubstitute 在这种情况下无法确定在哪个参数上应用匹配器
            //calculator.Received().Add(0, Arg.Any<int>());

            calculator.Received().Add(Arg.Is(0), Arg.Any<int>());
        }


        #endregion

        /*
         通常来讲，这个匹配器不是必须的；大部分情况下，我们可以使用 0 来代替 Arg.Is(0)。然而在某些情况下，NSubstitute 无法解析出那个匹配器应用到了那个参数上（实际上，参数匹配器进行的是模糊匹配；而不是直接解析函数的调用）。在这些情况下会抛出一个 AmbiguousArgumentsException，并且会要求你指定一个或多个额外的参数匹配器。大多数情况下你可能不得不为每个参数显式的使用参数匹配器。
         
         */




    }
}