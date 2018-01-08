using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;


namespace xUnit2.Demo.Tests.Parallel.Demo
{
    public class ParallelDemoTest
    {

        private readonly ITestOutputHelper _output;

        public ParallelDemoTest(ITestOutputHelper output)
        {
            _output = output;
        }


        [Fact]
        public void RunTest1()
        {
            System.Threading.Tasks.Parallel.Invoke(WatchMovie, HaveDinner, ReadBook, WriteBlog);
            _output.WriteLine("执行完成");
            //Console.ReadKey();
        }


        void WatchMovie()
        {
            Thread.Sleep(5000);
            _output.WriteLine("看电影");
        }
        void HaveDinner()
        {
            Thread.Sleep(1000);
            _output.WriteLine("吃晚饭");
        }
        void ReadBook()
        {
            Thread.Sleep(2000);
            _output.WriteLine("读书");
        }
        void WriteBlog()
        {
            Thread.Sleep(3000);
            _output.WriteLine("写博客");
        }



        private static int NUM_AES_KEYS = 1000;
        [Fact]
        public void RunTest2()
        {
            _output.WriteLine("执行" + NUM_AES_KEYS + "次：");
            GenerateAESKeys();
            ParallelGenerateAESKeys();
        }


        private void GenerateAESKeys()
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < NUM_AES_KEYS; i++)
            {
                var aesM = new AesManaged();
                aesM.GenerateKey();
                byte[] result = aesM.Key;
                string hexStr = ConverToHexString(result);
            }
            _output.WriteLine("AES:" + sw.Elapsed.ToString());
        }

        private void ParallelGenerateAESKeys()
        {
            var sw = Stopwatch.StartNew();
            System.Threading.Tasks.Parallel.For(1, NUM_AES_KEYS + 1, (int i) =>
            {
                var aesM = new AesManaged();
                aesM.GenerateKey();
                byte[] result = aesM.Key;
                string hexStr = ConverToHexString(result);
            });

            _output.WriteLine("Parallel_AES:" + sw.Elapsed.ToString());
        }

        string ConverToHexString(byte[] inpute)
        {
            Thread.Sleep(1);
            return "";
        }

        /// <summary>
        /// 在Parallel.For中，有时候对既有循环进行优化可能会是一个非常复杂的任务。
        /// Parallel.ForEach为固定数目的独立For Each循环迭代提供了负载均衡的并行执行，且支持自定义分区器，让使用者可以完全掌握数据分发。
        /// 实质就是将所有要处理的数据区分为多个部分，然后并行运行这些串行循环。
        /// </summary>
        [Fact]
        public void RunForEachTest()
        {
            var sw = Stopwatch.StartNew();
            System.Threading.Tasks.Parallel.ForEach(Partitioner.Create(1, NUM_AES_KEYS + 1), range =>
            {
                sw.Restart();
                var aesM = new AesManaged();
                var guid = Guid.NewGuid().ToString();
                _output.WriteLine($"[UGID:{guid}]AES Range({range.Item1},{range.Item2} 循环开始时间：{DateTime.Now.TimeOfDay})");
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    aesM.GenerateKey();
                    byte[] result = aesM.Key;
                    string hexStr = ConverToHexString(result);
                }
                _output.WriteLine($"[UGID:{guid}]AES:{sw.Elapsed.ToString()}");
            });
        }


        /*
         public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, Action<TSource> body)
         Parallel.ForEach方法定义了source和Body两个参数。
         source是指分区器。提供了分解为多个分区的数据源。
         body是要调用的委托。它接受每一个已定义的分区作为参数。一共有20多个重载，在上面的例子中，分区的类型为Tuple<int,int>,是一个二元组类型。此外，返回一个ParallelLoopResult的值。


        Partitioner.Create 创建分区是根据逻辑内核数及其他因素决定。

         
         */

    }
}