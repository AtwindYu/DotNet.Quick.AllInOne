using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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


        /*
         ** Parallel.ForEach也能对IEnumerable<int>集合进行重构。Enumerable.Range生产了序列化的数目。但这样就没有上面的分区效果。
         */

        [Fact]
        public void ParallelForEachGenerateMD5HasHes()
        {
            var sw = Stopwatch.StartNew();
            System.Threading.Tasks.Parallel.ForEach(Enumerable.Range(1, NUM_AES_KEYS), number =>
            {
                var md5M = MD5.Create();
                byte[] data = Encoding.Unicode.GetBytes(Environment.UserName + number);
                byte[] result = md5M.ComputeHash(data);
                string hexString = ConverToHexString(result);
                _output.WriteLine($"Number:{number}");
            });
            _output.WriteLine("MD5:" + sw.Elapsed.ToString() + ":");
        }




        /*从循环中退出
         
        和串行运行中的break不同，ParallelLoopState 提供了两个方法用于停止Parallel.For 和 Parallel.ForEach的执行。

        Break:让循环在执行了当前迭代后尽快停止执行。比如执行到100了，那么循环会处理掉所有小于100的迭代。
        Stop:让循环尽快停止执行。如果执行到了100的迭代，那不能保证处理完所有小于100的迭代。

         */
        [Fact]
        private void ParallelForEachGenerateMD5HasHesBreak()
        {
            var sw = Stopwatch.StartNew();
            var loopresult = System.Threading.Tasks.Parallel.ForEach(Enumerable.Range(1, NUM_AES_KEYS * 10), (int number, ParallelLoopState loopState) =>
              {
                  var md5M = MD5.Create();
                  byte[] data = Encoding.Unicode.GetBytes(Environment.UserName + number);
                  byte[] result = md5M.ComputeHash(data);
                  string hexString = ConverToHexString(result);
                  if (sw.Elapsed.Seconds > 1)
                  {
                      loopState.Stop();
                  }
              });
            ParallelLoopResult(loopresult);
            _output.WriteLine("MD5:" + sw.Elapsed);
        }

        private void ParallelLoopResult(ParallelLoopResult loopResult)
        {
            string text;
            if (loopResult.IsCompleted)
            {
                text = "循环完成";
            }
            else
            {
                if (loopResult.LowestBreakIteration.HasValue)
                {
                    text = "Break终止";
                }
                else
                {
                    text = "Stop 终止";
                }
            }
            _output.WriteLine(text);
        }



        /*
         捕捉并行循环中发生的异常。
          当并行迭代中调用的委托抛出异常，这个异常没有在委托中被捕获到时，就会变成一组异常，新的System.AggregateException负责处理这一组异常。
         */
        [Fact]
        public void ParallelForEachGenerateMD5HasHesException()
        {
            var sw = Stopwatch.StartNew();
            var loopresult = new ParallelLoopResult();
            try
            {
                loopresult = System.Threading.Tasks.Parallel.ForEach(Enumerable.Range(1, NUM_AES_KEYS * 10), (number, loopState) =>
                  {
                      var md5M = MD5.Create();
                      byte[] data = Encoding.Unicode.GetBytes(Environment.UserName + number);
                      byte[] result = md5M.ComputeHash(data);
                      string hexString = ConverToHexString(result);
                      if (sw.Elapsed.Seconds > 1)
                      {
                          throw new TimeoutException("执行超过1秒");
                      }
                  });
            }
            catch (AggregateException ex)
            {
                foreach (var innerEx in ex.InnerExceptions)
                {
                    _output.WriteLine(innerEx.ToString());
                }
            }

            ParallelLoopResult(loopresult);
            _output.WriteLine("MD5:" + sw.Elapsed);
        }




        /*指定并行度。
         
         TPL的方法总会试图利用所有可用的逻辑内核来实现最好的结果，但有时候你并不希望在并行循环中使用所有的内核。
         比如你需要留出一个不参与并行计算的内核，来创建能够响应用户的应用程序，而且这个内核需要帮助你运行代码中的其他部分。这个时候一种好的解决方法就是指定最大并行度。

        这需要创建一个ParallelOptions的实例，设置MaxDegreeOfParallelism的值。
         */

        [Theory]
        [InlineData(2)]
        public void ParallelMaxDegree(int maxDegree)
        {
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = maxDegree;

            var sw = Stopwatch.StartNew();
            System.Threading.Tasks.Parallel.For(1, NUM_AES_KEYS + 1, parallelOptions, (int i) =>
            {
                var aesM = new AesManaged();
                aesM.GenerateKey();
                byte[] result = aesM.Key;
                string hexStr = ConverToHexString(result);
            });
            _output.WriteLine("AES:" + sw.Elapsed.ToString());
        }

        [Fact]
        public void ParallelMaxDegreeRun()
        {
            ParallelMaxDegree(Environment.ProcessorCount - 1);
        }

        [Fact]
        public void EnumerableRunTest1()
        {
          var list =  Enumerable.Repeat(new User(), 10);
            _output.WriteLine($"{list.Count()}");



        }


        class User
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}