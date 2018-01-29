using System;
using System.Collections.Generic;
using Xunit;

namespace xUnit2.Demo.Tests.xUnit.Demo
{

    /// <summary>
    /// 1. 创建DateComparer类，实现IEqualityComparer接口定义的方法。
    /// 2. 在Assert.Equal 中使用对应的方法并传入相应的比较类（其中定义了比较逻辑）。
    /// </summary>
    class DateComparer : IEqualityComparer<DateTime>
    {
        public bool Equals(DateTime x, DateTime y)
        {
            return x.Date == y.Date;
        }

        public int GetHashCode(DateTime obj)
        {
            return obj.GetHashCode();
        }
    }

    /*
     * 　在实际的应用中，你可以使用Demo中的操作（很多公司也确实是这么做的）。
     * 　随着项目的演进（一段时间以后）你会发现代码中到处散落着实现了IEqualityComparer的对象，这样的维护成本可想而知。
     * 　建议的做法是对系统中IEqualityComparer类型统一封装，同时使用单件模式他们的构造进行控制。
     * 　
     * */


    /// <summary>
    /// 这里屏蔽了DateComparer的构造函数，并实现了单件模式的调用
    /// </summary>
    class DateComparer1 : IEqualityComparer<DateTime>
    {
        private DateComparer1() { }

        private static DateComparer1 _instance;
        public static DateComparer1 Instance => _instance ?? (_instance = new DateComparer1());

        public bool Equals(DateTime x, DateTime y)
        {
            return x.Date == y.Date;
        }

        public int GetHashCode(DateTime obj)
        {
            return obj.GetHashCode();
        }
    }

    /// <summary>
    /// 创建一个工厂类统一的控制所有单件比较类的创建逻辑
    /// <remarks>
    /// 其实，调用本身的代码量并没有减少（反而多了），那么我们为什么要这样实现呢？回到之前关于单元测试实践的讨论中所提到的。对于单元测试框架的设计我们的目的是什么？　
    /// 这里我列出来几个：
    /// 1. 提高开发效率（降低框架使用者的学习成本）。
    /// 2. 易于维护和管理。
    /// 3. 降低Test Case运行的时间成本
    /// </remarks>
    /// </summary>
    class SingletonFactory
    {
        public static DateComparer1 CreateDateComparer()
        {
            return DateComparer1.Instance;
        }
        //Other Comparer ... ...
    }



    public class ExampleForComparer
    {
        [Fact(DisplayName = "Assert.DateComparer.Demo01")]
        public void Assert_DateComparer_Demo01()
        {
            var firstTime = DateTime.Now.Date;
            var later = firstTime.AddMinutes(90);

            Assert.NotEqual(firstTime, later);
            Assert.Equal(firstTime, later, new DateComparer());
        }


        [Fact(DisplayName = "Assert.DateComparer1.Demo01")]
        public void Assert_DateComparer_Demo02()
        {
            var firstTime = DateTime.Now.Date;
            var later = firstTime.AddMinutes(90);

            Assert.NotEqual(firstTime, later);
            Assert.Equal(firstTime, later, SingletonFactory.CreateDateComparer());
        }
    }


}