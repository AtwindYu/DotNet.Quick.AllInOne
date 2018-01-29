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
            //����Ҫ�������л�ȡ�ض����ʱ��ֻ�����container.Resolve ���ɻ�ȡ�ض���ʵ����
            ILogger logger = WindsorInit.GetContainer().Resolve<ILogger>();
            logger.Debug("��¼��־");
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

            //            //CastleWindsor.IEntity��ʵ�������ڵĿռ�
            //            container.Register(Classes.FromThisAssembly().InNamespace("CastleWindsor.IEntity").WithService.DefaultInterfaces());

            //            //�̳������ӿ�
            //             container.Register(
            //             Component.For<IUserRepository, IRepository>()
            //             .ImplementedBy<MyRepository>()
            //            );

            //            //�򵥹���
            //            container
            //             .Register(
            //             Component.For<IMyService>()
            //             .UsingFactoryMethod(
            //             () => MyLegacyServiceFactory.CreateMyService())
            //             );

            //            //��������
            //            container.Register(
            //             Component.For(typeof(IRepository<>)
            //             .ImplementedBy(typeof(NHRepository<>)
            //            );

            //            //ʵ����������
            //            container.Register(
            //             Component.For<IMyService>()
            //             .ImplementedBy<MyServiceImpl>()
            //             .LifeStyle.Transient
            //            .Named("myservice.default")
            //             );

            //            //ȡ��ע���
            //            container.Register(
            //             Component.For<IMyService>().ImplementedBy<MyServiceImpl>(),
            //             Component.For<IMyService>().ImplementedBy<OtherServiceImpl>()
            //            );

            //            //ǿ��ȡ��ע���
            //            container.Register(
            //             Component.For<IMyService>().ImplementedBy<MyServiceImpl>(),
            //             Component.For<IMyService>().Named("OtherServiceImpl").ImplementedBy<OtherServiceImpl>().IsDefault()
            //            );

            //            //ע���Ѿ����ڵ�
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
