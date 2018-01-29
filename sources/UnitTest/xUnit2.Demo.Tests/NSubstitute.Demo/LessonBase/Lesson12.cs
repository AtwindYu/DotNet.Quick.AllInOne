using System;
using NSubstitute;
using Xunit;

namespace xUnit2.Demo.Tests.NSubstitute.Demo.LessonBase
{

    /// <summary>
    /// 使用回调函数和WhenDo语法
    /// </summary>
    public class Lesson12
    {

        //有时当收到一个特殊的调用时能执行一些代码是非常有用的。我们在使用函数设置返回值一节已经看到了类似的代码。
        public interface ICalculator
        {
            int Add(int a, int b);
            string Mode { get; set; }
        }

        [Fact]
        public void Test_CallbacksWhenDo_PassFunctionsToReturns()
        {
            var calculator = Substitute.For<ICalculator>();

            var counter = 0;
            calculator
                .Add(0, 0)
                .ReturnsForAnyArgs(x => 0)
                .AndDoes(x => counter++);

            calculator.Add(7, 3);
            calculator.Add(2, 2);
            calculator.Add(11, -3);
            Assert.Equal(3, counter);
        }


        //为无返回值调用创建回调
        //Returns() 可以被用于为成员设置产生返回值的回调函数，但是对于 void 类型的成员，我们需要不同的方式，因为我们无法调用一个 void 并返回一个值。对于这种情况，我们可以使用 When..Do 语法。


        #region 当被调用时，做这件事

        //When..Do 使用两个调用来配置回调。
        //首先，调用替代实例的 When() 方法来传递一个函数。
        //该函数的参数是替代实例自身，然后此处我们可以调用我们需要的成员，即使该成员返回 void。
        //然后再调用 Do() 方法来传递一个回调，当替代实例的成员被调用时，执行这个回调。

        public interface IFoo
        {
            void SayHello(string to);
        }

        [Fact]
        public void Test_CallbacksWhenDo_UseWhenDo()
        {
            var counter = 0;
            var foo = Substitute.For<IFoo>();

            foo.When(x => x.SayHello("World"))
                .Do(x => counter++);

            foo.SayHello("World_NOTCALLED");
            foo.SayHello("World");
            foo.SayHello("World");
            Assert.Equal(2, counter);
        }

        /*
         * 传递给 Do() 方法的参数中包含的调用信息与传递给 Returns() 回调的参数中的相同，这些调用信息可以用于对参数进行访问。
         * 注意，我们也可以对非 void 成员使用 When..Do 语法，但是，通常来说更加推荐 Returns() 语法，因为其更加简洁明确。
         * 你可能会发现，对于非 void 函数，当你想执行一个函数而不改变之前的返回值时，这个功能是非常有用的。*/

        [Fact]
        public void Test_CallbacksWhenDo_UseWhenDoOnNonVoid()
        {
            var calculator = Substitute.For<ICalculator>();

            var counter = 0;
            calculator.Add(1, 2).Returns(3);
            calculator
                .When(x => x.Add(Arg.Any<int>(), Arg.Any<int>()))
                .Do(x => counter++);

            var result = calculator.Add(1, 2);
            Assert.Equal(3, result);
            Assert.Equal(1, counter);
        }

        #endregion



        //为每个参数创建回调

        /*
         * 如果在某些地方，我们仅需要对一个特殊的参数创建回调，则我们可能会使用为每个参数创建回调的方法，例如 Arg.Do() 和 Arg.Invoke()，而不是使用 When..Do。
         * 参数回调给予我们更加简洁的代码，NSubstitute 中的其他 API 也保持这一风格。更多信息和示例，请查看在参数上执行操作一节。
         */


        #region LESSON 13 抛出异常

        //public interface ICalculator
        //{
        //    int Add(int a, int b);
        //    string Mode { get; set; }
        //}

        [Fact]
        public void Test_ThrowingExceptions_ForVoid()
        {
            var calculator = Substitute.For<ICalculator>();

            // 对无返回值函数
            calculator.Add(-1, -1).Returns(x => throw new Exception());

            // 抛出异常
            Assert.Throws<Exception>(() => calculator.Add(-1, -1));

        }

        [Fact]
        public void Test_ThrowingExceptions_ForNonVoidAndVoid()
        {
            var calculator = Substitute.For<ICalculator>();

            // 对有返回值或无返回值函数
            calculator
                .When(x => x.Add(-2, -2))
                .Do(x => throw new Exception());

            // 抛出异常
            //Assert.Throws<Exception>(() => calculator.Add(-1, -1));
            Assert.Throws<Exception>(() => calculator.Add(-2, -2));
        }


        #endregion

    }
}