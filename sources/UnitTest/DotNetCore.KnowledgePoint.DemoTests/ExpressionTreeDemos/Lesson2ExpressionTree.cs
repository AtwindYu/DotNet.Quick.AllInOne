using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetCore.KnowledgePoint.DemoTests.ExpressionTreeDemos
{

    /// <summary>
    /// ref:http://blog.jobbole.com/84588/
    /// </summary>
    [TestClass]
    //[Description("表达式树")]
    public class Lesson2ExpressionTree
    {
        /*
         表达式树也称表达式目录树，将代码以一种抽象的方式表示成一个对象树，树中每个节点本身都是一个表达式。
         表达式树不是可执行代码，它是一种数据结构。

        下面我们看看怎么通过C#代码建立一个表达式树。
         
         */




        [TestMethod]
        [Description("构建表达式树")]
        public void BuildExpressionTest()
        {
            /*
             System.Linq.Expressions命名空间中包含了代表表达式的各个类，所有类都从Expression派生，
             我们可以通过这些类中的静态方法来创建表达式类的实例。Expression类包括两个重要属性：

            Type属性代表求值表达式的.NET类型，可以把它视为一个返回类型
            NodeType属性返回所代表的表达式的类型
            下面看一个构建表达式树的简单例子：

             */

            Expression numA = Expression.Constant(6);
            Console.WriteLine("NodeType: {0}, Type: {1}", numA.NodeType, numA.Type);
            Expression numB = Expression.Constant(3);
            Console.WriteLine("NodeType: {0}, Type: {1}", numB.NodeType, numB.Type);

            BinaryExpression add = Expression.Add(numA, numB);
            Console.WriteLine("NodeType: {0}, Type: {1}", add.NodeType, add.Type);

            Console.WriteLine(add);
            //Console.Read();


            /*
             
             代码的输出为：

            
            通过例子可以看到，我们构建了一个(6+3)的表达式树，并且查看了各个节点的Type和NodeType属性。

            Expression有很多派生类，有很多节点类型。
            例如，BinaryExpression就代表了具有两个操作树的任意操作。
            这正是NodeType属性重要的地方，它能区分由相同的类表示的不同种类的表达式。
            其他的节点类型就不介绍了，有兴趣可以参考MSDN。

            对于上面的例子，可以用下图描述生成的表达式树，值得注意的是，”叶子”表达式在代码中是最先创建的，，表达式是自下而上构建的。
            表达式是不易变的，所有可以缓存和重用表达式。
             
             
             */


        }



        [TestMethod]
        [Description("将表达式编译成委托")]
        public void ComplaireExpressionToDelegateTest()
        {
            /*
        LambdaExpression是从Expression派生的类型之一。泛型类型Expression<TDelegate>又是从LambdaExpress派生的。

        Expression和Expression<TDelegate>的区别在于，泛型类以静态类型的方式标志了它是什么种类的表达式，也就是说，它确定了返回类型和参数。例如上面的加法例子，返回值是一个int类型，没有参数，所以我们可以使用签名Func<int>与之匹配，所以可以用Expression<Func<int>>以静态类型的方式来表示该表达式。

        这样做的目的在于，LambdaExpression有一个Compile方法，该方法能创建一个恰当类型的委托。 Expression<TDelegate>也有一个同名方法，该方法可以返回TDelegate类型的委托。获得了委托之后，我们就可以使 用普通委托实例调用的方式来执行这个表达式。

        接着上面加法的例子，我们把上面的加法表达式树转换成委托，然后执行委托：
             
             */

            Expression numA = Expression.Constant(6);
            Console.WriteLine("NodeType: {0}, Type: {1}", numA.NodeType, numA.Type);
            Expression numB = Expression.Constant(3);
            Console.WriteLine("NodeType: {0}, Type: {1}", numB.NodeType, numB.Type);

            BinaryExpression add = Expression.Add(numA, numB);
            Console.WriteLine("NodeType: {0}, Type: {1}", add.NodeType, add.Type);


            Func<int> addDelegate = Expression.Lambda<Func<int>>(add).Compile();
            Console.WriteLine(addDelegate());


            /*
             从这个例子中我们看到怎么构建一个表达式树，然后把这个对象树编译成真正的代码。
             在.NET 3.5中的表达式树只能是单一的表达式，不能表示完整的类、方法。
             这在.NET 4.0中得到了一定的改进，表达式树可以支持动态类型，我们可以创建块，为表达式赋值等等。
             */



        }



        [TestMethod]
        [Description("将Lambda表达式转换为表达式树")]
        public void LambdaExpressionToTreeTest()
        {
            /*
             Lambda表达式不仅可以创建委托实例，C# 3.0对于将Lambda表达式转换成表达式树提供了内建的支持。
             我们可以通过编译器把Lambda表达式转换成一个表达式树，并创建一个Expression<TDelegate>的一个实例。

             下面的例子中我们将一个Lambda表达式转换成一个表达式树，并通过代码查看表达式树的各个部分：

             */
             


            //将Lambda表达式转换为类型Expression<T>的表达式树
            //expression不是可执行代码
            Expression<Func<int, int, int>> expression = (a, b) => a + b;

            Console.WriteLine(expression);
            //获取Lambda表达式的主体
            BinaryExpression body = (BinaryExpression)expression.Body;
            Console.WriteLine(expression.Body);
            //获取Lambda表达式的参数
            Console.WriteLine(" param1: {0}, param2: {1}", expression.Parameters[0], expression.Parameters[1]);
            ParameterExpression left = (ParameterExpression)body.Left;
            ParameterExpression right = (ParameterExpression)body.Right;
            Console.WriteLine(" left body of expression: {0}{4} NodeType: {1}{4} right body of expression: {2}{4} Type: {3}{4}", left.Name, body.NodeType, right.Name, body.Type, Environment.NewLine);

            //将表达式树转换成委托并执行
            Func<int, int, int> addDelegate = expression.Compile();
            Console.WriteLine(addDelegate(10, 16));

            /*
             
             (a, b) => (a + b)
(a + b)
 param1: a, param2: b
 left body of expression: a
 NodeType: Add
 right body of expression: b
 Type: System.Int32

26
             
             */

        }


        /*
         
表达式树的用途

前面看到，通过Expression的派生类中的各种节点类型，我们可以构建表达式树；
然后可以把表达式树转换成相应的委托类型实例，最后执行委托实例的代码。
但是，我们不会绕这么大的弯子来执行委托实例的代码。

表达式树主要在LINQ to SQL中使用，我们需要将LINQ to SQL查询表达式（返回IQueryable类型）转换成表达式树。
之所以需要转换是因为LINQ to SQL查询表达式不是在C#代码中执行的，LINQ to SQL查询表达式被转换成SQL，通过网络发送，最后在数据库服务器上执行。

这里只做个简单的介绍，后续会介绍LINQ to SQL相关的内容。

编译器对Lambda表达式的处理
前面我们了解到，Lambda可以用来创建委托实例，也可以用来生成表达式树，这些都是编译器帮我们完成的。

编译器如何决定生成可执行的IL还是一个表达式树：

当Lambda表达式赋予一个委托类型的变量时，编译器生成与匿名方法同样的IL（可执行的委托实例）
当Lambda表达式赋予一个Expression类型的变量时，编译器就将它转换成一个表达式树
下图展示了LINQ to Object和LINQ to SQL中Lambda表达式的不同处理方式：



总结
本文中介绍了Lambda表达式，在匿名方法的基础上进一步简化了委托实例的创建，编写更加简洁、易读的代码。匿名函数不等于匿名方法，匿名函数包含了匿名方法和lambda表达式这两种概念

Lambda不仅可以创建委托实例，还可以由编译器转换成表达式树，使代码可以在程序之外执行（参考LINQ to SQL）。
         
         
         */


    }
}