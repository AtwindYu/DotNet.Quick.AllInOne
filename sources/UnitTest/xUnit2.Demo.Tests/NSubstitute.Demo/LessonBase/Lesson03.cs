using System.Globalization;
using NSubstitute;
using Xunit;

namespace xUnit2.Demo.Tests.NSubstitute.Demo.LessonBase
{
    /// <summary>
    /// 设置返回值
    /// </summary>
    public class Lesson03
    {

        /// <summary>
        /// 要为替代实例上的一个方法调用设置返回值，可以按正常调用该方法，然后紧跟着一个 NSubstitute 的 Returns() 扩展方法。
        /// </summary>
        [Fact]
        public void Test_SettingReturnValue_ReturnsValueWithSpecifiedArguments()
        {
            var calculator = Substitute.For<ICalculator>();
            calculator.Add(1, 2).Returns(3);
            Assert.Equal(3, calculator.Add(1, 2));
        }

        /// <summary>
        /// 该方法在每次被调用时都会返回这个值。
        /// Returns() 仅会被应用于指定的参数组合，任何使用其他参数组合对该方法的调用将会返回一个默认值。
        /// </summary>
        [Fact]
        public void Test_SettingReturnValue_ReturnsDefaultValueWithDifferentArguments()
        {
            var calculator = Substitute.For<ICalculator>();

            // 设置调用返回值为3
            calculator.Add(1, 2).Returns(3);

            Assert.Equal(3, calculator.Add(1, 2));
            Assert.Equal(3, calculator.Add(1, 2));

            // 当使用不同参数调用时,返回值不是3
            Assert.Equal(3,calculator.Add(3, 6) );
        }

        /// <summary>
        /// 为属性设置返回值的方式与为方法设置是一样的，仍然使用 Returns() 语法。对于可读写属性，可以使用传统的属性 setter 进行设置，当然结果就像你期待的那样。
        /// </summary>
        [Fact]
        public void Test_SettingReturnValue_ReturnsValueFromProperty()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Mode.Returns("DEC");
            Assert.Equal("DEC", calculator.Mode);

            calculator.Mode = "HEX";
            Assert.Equal("HEX", calculator.Mode);
        }

        #region 为特定参数设置返回值

      
        /// <summary>
        /// 通过使用参数匹配器，可以配置将不同的组合参数传递至方法调用。在参数匹配器一节将更为详细的介绍这一功能。下面的示例介绍了一般用法：
        /// </summary>
        [Fact]
        public void Test_ReturnForSpecificArgs_UseArgumentsMatcher()
        {
            var calculator = Substitute.For<ICalculator>();

            // 当第一个参数是任意int类型的值，第二个参数是5时返回。
            calculator.Add(Arg.Any<int>(), 5).Returns(10);
            Assert.Equal(10, calculator.Add(123, 5));
            Assert.Equal(10, calculator.Add(-9, 5));
            Assert.NotEqual(10, calculator.Add(-9, -9));

            // 当第一个参数是1，第二个参数小于0时返回。
            calculator.Add(1, Arg.Is<int>(x => x < 0)).Returns(345);
            Assert.Equal(345, calculator.Add(1, -2));
            Assert.NotEqual(345, calculator.Add(1, 2));

            // 当两个参数都为0时返回。
            calculator.Add(Arg.Is(0), Arg.Is(0)).Returns(99);
            Assert.Equal(99, calculator.Add(0, 0));
        }

        #endregion


        #region 为任意参数设置返回值

        /// <summary>
        /// 通过使用 ReturnsForAnyArgs() 方法，可以设置当一个方法被调用后，无论参数是什么，都返回指定的值。
        /// 同样的行为也可以通过参数匹配器来达成：可简单快捷地通过 Arg.Any<T>() 来替换每个参数。
        /// ReturnsForAnyArgs() 具有与 Returns() 方法相同的重载，所以也可以指定多个返回值，或者计算返回值。
        /// </summary>
        [Fact]
        public void Test_ReturnForAnyArgs_ReturnForAnyArgs()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Add(1, 2).ReturnsForAnyArgs(100);
            Assert.Equal(100, calculator.Add(1, 2));
            Assert.Equal(100, calculator.Add(-7, 15));
        }



        #endregion


      






    }
}