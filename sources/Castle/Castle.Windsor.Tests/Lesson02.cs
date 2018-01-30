using System;
using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.Windsor.Tests
{

    /// <summary>
    /// AOP就是可以在一个已有的类方法中动态地嵌入代码，可以通过预编译方式和运行期动态代理实现在不修改源代码的情况下给程序动态统一添加功能。
    /// 前提必须是被切入的类是通过IOC容器来控制的。
    /// 
    /// <remarks>
    ///  Castle通过DynamicProxy来实现动态代理每一个切面方法均需要实现接口IInterceptor。
    /// </remarks>
    /// 
    /// </summary>
    [TestClass]
    public class Lesson02
    {

        [TestMethod]
        public void Run1()
        {

            //直接调用是不行的。
            var ser = new PersonAppService();
            ser.CreatePerson("atwind",10000);

        }


        [TestMethod]
        public void Run2()
        {

            //这样才会正常的解析出来
            var container = new WindsorContainer();
            container.Register(Component.For<IAppService>().ImplementedBy<PersonAppService>());
            container.Register(Component.For<IInterceptor>().ImplementedBy<LoggingInterceptor>());

            var ser2 = container.Resolve<IAppService>();
            ser2.CreatePerson("atwind", 99999);

            container.Dispose();
        }

    }



    //这样在PersonAppService每个方法外面均会套上TryCatch并记录日志。
    [Interceptor(typeof(LoggingInterceptor))]
    public class PersonAppService: IAppService
    {
        public void CreatePerson(string name, int age)
        {
            Console.WriteLine($"{name}-{age}");
        }
    }

    public interface IAppService
    {
        void CreatePerson(string name, int age);
    }

    //下面通过代码的方式来学习下AOP,新建一个切入类
    //即在原来的方法中加入TryCatch块，并记录日志。客户端调用的时候只需要在类上加标签：
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("方法前调用");
            try
            {
                invocation.Proceed();
            }
            catch (Exception)
            {
                Console.WriteLine("方法出错调用");
                throw;
            }
            finally
            {
                Console.WriteLine("方法最后调用");
            }
        }
    }
}