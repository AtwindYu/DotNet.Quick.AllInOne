using Xunit;

namespace xUnit2.Demo.Tests.xUnit.Demo
{
    /// <summary>
    /// 一组类的共享
    /// 
    /// 定义Collection名称，标明使用的Fixture
    /// <remarks>
    /// 该类的主要功能是定义了一个名字为“DatabaseCollection”（此名称可以和类名不同）的Collection，并指明该Collection所对应了Fixture。
    /// 需要说明的是ICollectionFixture和IClassFixture一样是一个泛型标记接口（即没有任何需要实现的方法，只是用来标记对应的Fixture的类型)。
    /// 
    /// 被CollectionDefinition标记的Class在运行时会被xUnit.Net框架实例化为一个对象，该对象将用于标记其他的Class（有兴趣的话可以去GitHub看看xUnit.Net的源代码）。
    /// 这里需要一个CollectionName作为参数，该参数将会用标记那些需要使用这个CollectionFixture的类。
    /// </remarks>
    /// </summary>
    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}