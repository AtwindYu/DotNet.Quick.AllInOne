using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetCore.KnowledgePoint.DemoTests.ExpressionTreeDemos
{

    [TestClass]
    public class Lesson1
    {






        [TestMethod]
        [Description("表达式树")]

        public void ExpressionTreeDemo1()
        {

        }

        [TestMethod]
        [Description("在List<T>中使用Lambda表达式")]
        public void UseLambadaInListTest()
        {
            /*
             前面简单的介绍了什么是Lambda表达式，下面通过一个例子进一步了解Lambda表达式。

在前面的文章中，我们也提到了一下List<T>的方法，例如FindAll方法，
参数是Predicate<T>类型的 委托，返回结果是一个筛选后的新列表；
Foreach方法获取一个Action<T>类型的委托，然后对每个元素设置行为。
下面就看看在 List<T>中使用Lambda表达式：

             */

            var books = new List<Book>
            {
                new Book{Name="C# learning guide",Year=2005},
                new Book{Name="C# step by step",Year=2005},
                new Book{Name="Java learning guide",Year=2004},
                new Book{Name="Java step by step",Year=2004},
                new Book{Name="Python learning guide",Year=2003},
                new Book{Name="C# in depth",Year=2012},
                new Book{Name="Java in depth",Year=2014},
                new Book{Name="Python in depth",Year=2013},
            };

            //创建一个委托实例来表示一个通用的操作
            //Action<Book> printer = book => Console.WriteLine("Name = {0}, Year = {1}", book.Name, book.Year);
            void printer(Book book) => Console.WriteLine("Name = {0}, Year = {1}", book.Name, book.Year);

            books.ForEach(printer);

            //使用Lambda表达式对List<T>进行筛选
            books.FindAll(book => book.Year > 2010).ForEach(printer);

            books.FindAll(book => book.Name.Contains("C#")).ForEach(printer);

            //使用Lambda表达式对List<T>进行排序
            books.Sort((book1, book2) => book1.Name.CompareTo(book2.Name));
            books.ForEach(printer);

            //Console.Read();

            /*
             
             从上面例子可以看到，当我们要经常使用一个操作的时候，我们最好创建一个委托实例，然后反复调用，而不是每次使用的时候都使用Lambda表达式（例如例子中的printer委托实例）。

             */


        }
        public class Book
        {
            public string Name { get; set; }
            public int Year { get; set; }
        }



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