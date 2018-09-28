using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsTestv2.Demo.Tests.EmitDemos
{

    [TestClass]
    public class Demo1
    {

        [TestMethod]
        public void Run1()
        {
            //首先需要声明一个程序集名称
            // specify a new assembly name
            var assemblyName = new AssemblyName("Kitty");

            //从当前应用程序域获取程序集构造器，
            // create assembly builder
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            //在程序集中构造动态模块
            // create module builder
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("KittyModule", "Kitty.exe");
            //模块即是代码的集合，一个程序集中可以有多个模块。并且理论上讲，每个模块可以使用不同的编程语言实现，例如C#/VB。

            // 构造一个类型构造器，
            // create type builder for a class
            var typeBuilder = moduleBuilder.DefineType("HelloKittyClass", TypeAttributes.Public);

            //通过类型构造器定义一个方法，获取方法构造器，获得方法构造器的IL生成器，通过编写IL代码来定义方法功能。
            // create method builder
            var methodBuilder = typeBuilder.DefineMethod("SayHelloMethod", MethodAttributes.Public | MethodAttributes.Static, null, null);
            // then get the method il generator
            var il = methodBuilder.GetILGenerator();
            // then create the method function
            il.Emit(OpCodes.Ldstr, "Hello, Kitty!");
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine"));
            il.Emit(OpCodes.Pop); // we just read something here, throw it.
            il.Emit(OpCodes.Ret);

            //创建类型，
            // then create the whole class type
            var helloKittyClassType = typeBuilder.CreateType();

            //如果当前程序集是可运行的，则设置一个程序入口，
            // set entry point for this assembly
            assemblyBuilder.SetEntryPoint(helloKittyClassType.GetMethod("SayHelloMethod"));

            //将动态生成的程序集保存成磁盘文件，
            // save assembly
            assemblyBuilder.Save("Kitty.exe");





        }


    }
}