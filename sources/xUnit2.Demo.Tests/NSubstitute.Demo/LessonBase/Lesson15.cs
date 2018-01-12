using NSubstitute;
using Xunit;

namespace xUnit2.Demo.Tests.NSubstitute.Demo.LessonBase
{
    /// <summary>
    /// 自动递归模拟
    /// </summary>
    public class Lesson15
    {
        //替代实例一旦被设置属性或方法，则将自动返回非NULL值。
        //例如，任何属性或方法如果返回接口、委托或纯虚类*，则将自动的返回替代实例自身。
        //通常这被称为递归模拟技术，而且是非常实用的。
        //比如其可以避免显式地设置每个替代实例，也就意味着更少量的代码。诸如String和Array等类型，默认会返回空值而不是NULL。

        //*注：一个纯虚类是指一个类，其所有的公有方法和属性都被定义为virtual或abstract，并且其具有一个默认公有或受保护地无参构造函数。

        #region 递归模拟

        public interface INumberParser
        {
            int[] Parse(string expression);
        }
        public interface INumberParserFactory
        {
            INumberParser Create(char delimiter);
        }


        //我们想配置 INumberParserFactory 来创建一个解析器，该解析器会为一个 expression 返回一定数量的 int 类型的值。我们可以手工创建每个替代实例：

        [Fact]
        public void Test_AutoRecursiveMocks_ManuallyCreateSubstitutes()
        {
            var factory = Substitute.For<INumberParserFactory>();
            var parser = Substitute.For<INumberParser>();
            factory.Create(',').Returns(parser);
            parser.Parse("an expression").Returns(new int[] { 1, 2, 3 });

            var actual = factory.Create(',').Parse("an expression");
            Assert.Equal(new int[] { 1, 2, 3 }, actual);
        }

        //或者可以应用递归模拟功能，INumberParserFactory.Create() 会自动返回 INumberParser 类型的替代实例。

        [Fact]
        public void Test_AutoRecursiveMocks_AutomaticallyCreateSubstitutes()
        {
            var factory = Substitute.For<INumberParserFactory>();
            factory.Create(',').Parse("an expression").Returns(new int[] { 1, 2, 3 });

            var actual = factory.Create(',').Parse("an expression");
            Assert.Equal(new int[] { 1, 2, 3 }, actual);
        }

        //每次当使用相同参数调用一个被递归模拟的属性或方法时，都会返回相同的替代实例。如果使用不同参数调用，则将会返回一个新的替代实例。


        [Fact]
        public void Test_AutoRecursiveMocks_CallRecursivelySubbed()
        {
            var factory = Substitute.For<INumberParserFactory>();
            factory.Create(',').Parse("an expression").Returns(new int[] { 1, 2, 3 });

            var firstCall = factory.Create(',');
            var secondCall = factory.Create(',');
            var thirdCallWithDiffArg = factory.Create('x');

            Assert.Same(firstCall, secondCall);
            Assert.NotSame(firstCall, thirdCallWithDiffArg);
        }

        //注：不会为类创建递归的替代实例，因为创建和使用类可能有潜在的或多余的副作用。因此，有必要显式地创建和返回类的替代实例。

        #endregion


        #region 替代链

        //当需要时，我们可以使用递归模拟来简单地设置替代链，但这并不是一个理想的做法。例如：
        public interface IContext
        {
            IRequest CurrentRequest { get; }
        }
        public interface IRequest
        {
            IIdentity Identity { get; }
            IIdentity NewIdentity(string name);
        }
        public interface IIdentity
        {
            string Name { get; }
            string[] Roles();
        }
        //如果要获取 CurrentRequest 中的 Identity 并返回一个名字，我们可以手工为 IContext、IRequest 和 IIdentity 创建替代品，然后使用 Returns() 将这些替代实例链接到一起。
        //或者我们可以使用为属性和方法自动创建的替代实例。
        [Fact]
        public void Test_AutoRecursiveMocks_SubstituteChains()
        {
            var context = Substitute.For<IContext>();
            context.CurrentRequest.Identity.Name.Returns("My pet fish Eric");
            Assert.Equal(
                "My pet fish Eric",
                context.CurrentRequest.Identity.Name);
        }

        //在这里 CurrentReques t是自动返回一个 IRequest 的替代实例，IRequest 替代实例会自动返回一个 IIdentity 替代实例。

        //注：类似于这种设置很长的替代实例链，一般被认为是代码臭味：我们打破了 Law of Demeter 原则，对象只应该与其直接关系的临近对象打交道，而不与临近对象的临近对象打交道。
        //如果你写的测试用例中没有使用递归模拟，设置的过程可能会明显的变复杂，所以如果要使用递归模式，则需要格外的注意类似的类型耦合。

        #endregion

        #region 自动值

        //当属性或方法返回 String 或 Array 类型的值时，默认会返回空或者非 NULL 值。比如在你仅需要返回一个对象引用，但并不关心其特定的属性时，这个功能可以帮你避免空引用异常。
        [Fact]
        public void Test_AutoRecursiveMocks_AutoValues()
        {
            var identity = Substitute.For<IIdentity>();
            Assert.Equal(string.Empty, identity.Name);
            Assert.Empty(identity.Roles());
        }


        #endregion



        #region Lesson16 设置out和ref参数

        //通过使用 Returns() 回调或者 When..Do 语法可以设置 out 和 ref 的参数。
        public interface ILookup
        {
            bool TryLookup(string key, out string value);
        }

        //对于上面的接口，我们可以配置其返回值，并设置第二个参数的输出：

        [Fact]
        public void Test_SetOutRefArgs_SetOutArg()
        {
            // Arrange
            var value = "";
            var lookup = Substitute.For<ILookup>();
            lookup
                .TryLookup("hello", out value)
                .Returns(x =>
                {
                    x[1] = "world!"; //设置第二个参数的输出
                    return true;
                });

            // Act
            var result = lookup.TryLookup("hello", out value);

            // Assert
            Assert.True(result);
            Assert.Equal("world!", value);
        }



        #endregion

    }
}