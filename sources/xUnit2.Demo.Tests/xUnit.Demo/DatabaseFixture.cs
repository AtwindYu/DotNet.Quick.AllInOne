using System;

namespace xUnit2.Demo.Tests.xUnit.Demo
{

    /// <summary>
    /// 省略了得创建和销毁数据库连接的Code。只是使用了一个object类型的属性来表示数据库上下文，并且创建了一个静态变量ExecuteCount用于标记构造函数的使用频率。
    /// </summary>
    public class DatabaseFixture : IDisposable
    {
        public object DatabaseContext { get; set; }

        public static int ExecuteCount { get; set; }

        public DatabaseFixture()
        {
            ExecuteCount++;
            //初始化数据连接
        }

        public void Dispose()
        {
            //销毁数据连接
        }
    }
}