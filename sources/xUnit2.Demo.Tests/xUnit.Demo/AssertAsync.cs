using System;
using System.Threading.Tasks;
using Xunit;

namespace xUnit2.Demo.Tests.xUnit.Demo
{
    public class AssertAsync
    {
        [Fact]
        public async void CodeThrowsAsync()
        {
            Func<Task> testCode = () => Task.Factory.StartNew(ThrowingMethod);

            var ex = await Assert.ThrowsAsync<NotImplementedException>(testCode);

            Assert.IsType<NotImplementedException>(ex);
        }

        [Fact]
        public async void RecordAsync()
        {
            Func<Task> testCode = () => Task.Factory.StartNew(ThrowingMethod);

            var ex = await Record.ExceptionAsync(testCode);

            Assert.IsType<NotImplementedException>(ex);
        }

        void ThrowingMethod()
        {
            throw new NotImplementedException();
        }

        /*
         
        如上面的Code所示，使用xUnit.Net编写异步处理相关的Unit Test，一般有以下几个步骤：
        
        1. Test Case 方法标记为 ： async
        2. 定义待测试的方法
        3. 使用Assert.ThrowsAsync或者Record.ExceptionAsync来执行线程操作
        4. 判断结果

         */


    }
}