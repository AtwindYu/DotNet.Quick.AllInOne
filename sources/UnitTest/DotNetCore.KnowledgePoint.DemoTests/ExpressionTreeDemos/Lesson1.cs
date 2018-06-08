using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetCore.KnowledgePoint.DemoTests.ExpressionTreeDemos
{

    [TestClass]
    public class Lesson1
    {




        [TestMethod]
        public void GetWordsLengthTest()
        {
            //使用C# 2.0中的匿名方法获取字符串长度
            Func<string, int> strLength = delegate (string str) { return str.Length; };
            Console.WriteLine(strLength("Hello World!"));

            //使用Lambda表达式
            //（显式类型参数列表）=> {语句}，lambda表达式最冗长版本
            strLength = (string str) => { return str.Length; };
            Console.WriteLine(strLength("Hello World!"));

            //单一表达式作为主体
            //（显式类型参数列表）=> 表达式
            strLength = (string str) => str.Length;
            Console.WriteLine(strLength("Hello World!"));

            //隐式类型的参数列表
            //（隐式类型参数列表）=> 表达式
            strLength = (str) => str.Length;
            Console.WriteLine(strLength("Hello World!"));

            //单一参数的快捷语法
            //参数名 => 表达式
            strLength = str => str.Length;
            Console.WriteLine(strLength("Hello World!"));



            //“=>”是C# 3.0新增的，告诉编译器我们正在使用Lambda表达式。
            //”=>”可以读作”goes to”，所以例子中的Lambda表达式可以读作”str goes to str.Length”。
            //从例子中还可以看到，根据Lambda使用的特殊情况，我们可以进一步简化Lambda表达式。



            /*
             Lambda表达式大多数时候都是和一个返回非void的委托类型配合使用（例如Func<TResult>）。
             在C# 1.0中，委托一般用于事件，很少会返回什么结果。
             
            在LINQ中，委托通常被视为数据管道的一部分，接受输入并返回结果，或者判断某项是否符合当前的筛选 器等等。
             
             */


            /*
             Lambda表达式本质

通过ILSpy查看上面的例子，可以发现Lambda表达式就是匿名方法，是编译器帮我们进行了转换工作，使我们可以直接使用Lambda表达式来进一步简化创建委托实例的代码。
             
             */

        }






        [TestMethod]
        [Description("表达式树的ToString方法是丢失信息的")]
        public void WhatIsToStringForExpression()
        {
            Expression<Func<int, int>> exp1 = i => i;
            Expression<Func<long, long>> exp2 = i => i;


            Console.WriteLine(exp1.ToString());
            Console.WriteLine(exp2.ToString());


           

        }
    }
}