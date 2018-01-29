using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.Windsor.Tests
{
    [TestClass]
    public class Lesson01
    {
        [TestMethod]
        public void Setp01_Init_And_Resolve_It()
        {
            //在需要从容器中获取特定类的时候，只需调用container.Resolve 即可获取特定的实现类
            ILogger logger = WindsorInit.GetContainer().Resolve<ILogger>();
            logger.Debug("记录日志");
        }
    }



    public class WindsorInit
    {
        private static WindsorContainer _container;
        public static WindsorContainer GetContainer()
        {
            if (_container == null)
            {
                _container = new WindsorContainer();
                _container.Install(
                    new MyInstaller()
                );
            }
            return _container;
        }

        public void CloseContex()
        {
            _container.Dispose();
        }
    }

    public class MyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILogger>().ImplementedBy<ConsoleLogger>().LifestyleSingleton());

            //            //CastleWindsor.IEntity是实现类所在的空间
            //            container.Register(Classes.FromThisAssembly().InNamespace("CastleWindsor.IEntity").WithService.DefaultInterfaces());

            //            //继承两个接口
            //             container.Register(
            //             Component.For<IUserRepository, IRepository>()
            //             .ImplementedBy<MyRepository>()
            //            );

            //            //简单工厂
            //            container
            //             .Register(
            //             Component.For<IMyService>()
            //             .UsingFactoryMethod(
            //             () => MyLegacyServiceFactory.CreateMyService())
            //             );

            //            //泛型配置
            //            container.Register(
            //             Component.For(typeof(IRepository<>)
            //             .ImplementedBy(typeof(NHRepository<>)
            //            );

            //            //实体生命周期
            //            container.Register(
            //             Component.For<IMyService>()
            //             .ImplementedBy<MyServiceImpl>()
            //             .LifeStyle.Transient
            //            .Named("myservice.default")
            //             );

            //            //取先注册的
            //            container.Register(
            //             Component.For<IMyService>().ImplementedBy<MyServiceImpl>(),
            //             Component.For<IMyService>().ImplementedBy<OtherServiceImpl>()
            //            );

            //            //强制取后注册的
            //            container.Register(
            //             Component.For<IMyService>().ImplementedBy<MyServiceImpl>(),
            //             Component.For<IMyService>().Named("OtherServiceImpl").ImplementedBy<OtherServiceImpl>().IsDefault()
            //            );

            //            //注册已经存在的
            //            var customer = new CustomerImpl();
            //            container.Register(
            //             Component.For<ICustomer>().Instance(customer)
            //             );

        }
    }


    //public interface IWindsorInstaller
    //{
    //    void Install(IWindsorContainer container, IConfigurationStore store);
    //}





    public interface ILogger
    {
        void Debug(string msg);
    }

    public class ConsoleLogger : ILogger
    {
        public void Debug(string msg)
        {
            Console.WriteLine("Console Debug :" + msg);
        }
    }
}
