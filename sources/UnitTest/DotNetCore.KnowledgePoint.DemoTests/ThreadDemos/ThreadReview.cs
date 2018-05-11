using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetCore.KnowledgePoint.DemoTests
{

    //https://www.cnblogs.com/jesse2013/p/async-and-await.html



    [TestClass]
    public class ThreadReview
    {
        [TestMethod]
        public void TestMethod1()
        {
        }



        #region 创建

        [TestMethod]
        public void ThreadDemo1()
        {
            new Thread(ThreadGo).Start();  // .NET 1.0开始就有的
            Task.Factory.StartNew(ThreadGo); // .NET 4.0 引入了 TPL
            Task.Run(new Action(ThreadGo)); // .NET 4.5 新增了一个Run的方法

            //创建Thread的实例之后，需要手动调用它的Start方法将其启动。
            //但是对于Task来说，StartNew和Run的同时，既会创建新的线程，并且会立即启动它。

            // Thread.Sleep(500); //为了看到输出的等待
        }

        public static void ThreadGo()
        {
            Thread.Sleep(200);
            Console.WriteLine("我是另一个线程");
        }


        #endregion



        #region 线程池


        [TestMethod]
        public void ThreadPoolDemo1()
        {
            Console.WriteLine("我是主线程：Thread Id {0}", Thread.CurrentThread.ManagedThreadId);
            ThreadPool.QueueUserWorkItem(ThreadPoolGo);

            //线程的创建是比较占用资源的一件事情，.NET 为我们提供了线程池来帮助我们创建和管理线程。
            //Task是默认会直接使用线程池，但是Thread不会。
            //如果我们不使用Task，又想用线程池的话，可以使用ThreadPool类。

            //Console.ReadLine();
        }

        public static void ThreadPoolGo(object data)
        {
            Console.WriteLine("我是另一个线程:Thread Id {0}", Thread.CurrentThread.ManagedThreadId);
        }



        #endregion



        #region 传入参数

        [TestMethod]
        public void InputParamDemo1()
        {
            new Thread(InputParamGo).Start("arg1"); // 没有匿名委托之前，我们只能这样传入一个object的参数

            new Thread(delegate ()
            {  // 有了匿名委托之后...
                InputParamGoGoGo("arg1", "arg2", "arg3");
            });

            new Thread(() =>
            {  // 当然,还有 Lambada
                InputParamGoGoGo("arg1", "arg2", "arg3");
            }).Start();

            Task.Run(() =>
            {  // Task能这么灵活，也是因为有了Lambda呀。
                InputParamGoGoGo("arg1", "arg2", "arg3");
            });
        }

        public static void InputParamGo(object name)
        {
            // TODO
        }

        public static void InputParamGoGoGo(string arg1, string arg2, string arg3)
        {
            // TODO
        }

        #endregion



        #region 返回值

        [TestMethod]
        public void ReuntnValueDemo1()
        {
            // GetDayOfThisWeek 运行在另外一个线程中
            var dayName = Task.Run<DateTime>(() => { return DateTime.Now; });
            Console.WriteLine("今天是：{0}", dayName.Result);

            //Thead是不能返回值的，但是作为更高级的Task当然要弥补一下这个功能。
        }


        #endregion



        #region 共享数据


        private static bool _isDone = false;

        [TestMethod]
        public void ShareDataDemo1()
        {
            new Thread(ShareDataDone).Start();
            new Thread(ShareDataDone).Start();
        }

        static void ShareDataDone()
        {
            if (!_isDone)
            {
                _isDone = true; // 第二个线程来的时候，就不会再执行了(也不是绝对的，取决于计算机的CPU数量以及当时的运行情况)
                Console.WriteLine("Done");
            }
        }

        //线程之间可以通过static变量来共享数据。

        #endregion



        #region 线程安全


        //private static bool _isDone = false;
        [TestMethod]
        public void ThreadSafeDemo1()
        {
            new Thread(ThreadSafeDone).Start();
            new Thread(ThreadSafeDone).Start();
            //Console.ReadLine();
            Thread.Sleep(200);
        }

        static void ThreadSafeDone()
        {
            if (!_isDone)
            {
                Console.WriteLine("Done"); // 猜猜这里面会被执行几次？
                _isDone = true;
            }
        }

        //上面这种情况不会一直发生，但是如果你运气好的话，就会中奖了。
        //因为第一个线程还没有来得及把_isDone设置成true，第二个线程就进来了，而这不是我们想要的结果，
        //在多个线程下，结果不是我们的预期结果，这就是线程不安全。


        #endregion


        #region 锁

        //要解决上面遇到的问题，我们就要用到锁。
        //锁的类型有独占锁，互斥锁，以及读写锁等，我们这里就简单演示一下独占锁。

        //private static bool _isDone = false;
        private static object _lock = new object();
        [TestMethod]
        public void LockDemo1()
        {
            new Thread(LockDone).Start();
            new Thread(LockDone).Start();
            //Console.ReadLine();
            Thread.Sleep(200);
        }

        static void LockDone()
        {
            lock (_lock)
            {
                if (!_isDone)
                {
                    Console.WriteLine("Done"); // 猜猜这里面会被执行几次？
                    _isDone = true;
                }
            }
        }

        //再我们加上锁之后，被锁住的代码在同一个时间内只允许一个线程访问，其它的线程会被阻塞，只有等到这个锁被释放之后其它的线程才能执行被锁住的代码。

        #endregion


        #region Semaphore 信号量


        //它可以控制对某一段代码或者对某个资源访问的线程的数量，超过这个数量之后，其它的线程就得等待，只有等现在有线程释放了之后，下面的线程才能访问。
        //这个跟锁有相似的功能，只不过不是独占的，它允许一定数量的线程同时访问。

        static SemaphoreSlim _sem = new SemaphoreSlim(3);    // 我们限制能同时访问的线程数量是3
        [TestMethod]
        public void SemaphoreDemo1()
        {
            for (int i = 1; i <= 5; i++) new Thread(SemaphoreEnter).Start(i);
            //Console.ReadLine();

            Thread.Sleep(8000);

        }

        static void SemaphoreEnter(object id)
        {
            Console.WriteLine(id + $" 开始排队... [{DateTime.Now:HH:mm:ss ffffff}]");
            _sem.Wait();
            Console.WriteLine(id + $" 开始执行！ [{DateTime.Now:HH:mm:ss ffffff}]");
            Thread.Sleep(1000 * (int)id);
            Console.WriteLine(id + $" 执行完毕，离开！[{DateTime.Now:HH:mm:ss ffffff}]");
            _sem.Release();
        }


        #endregion



        #region 异常处理   --　其它线程的异常，主线程可以捕获到么？

        [TestMethod]
        public void ThreadExceptionDemo1()
        {
            try
            {
                new Thread(ThreadExceptionGo).Start();
            }
            catch (Exception ex)
            {
                // 其它线程里面的异常，我们这里面是捕获不到的。
                Console.WriteLine("Exception!");
            }
            Thread.Sleep(200);
        }
        static void ThreadExceptionGo() { throw null; }

        //那么升级了的Task呢？
        [TestMethod]
        public void TaskExecptionDemo1()
        {
            try
            {
                var task = Task.Run(() => { TaskExecptionGo(); });
                task.Wait();  // 在调用了这句话之后，主线程才能捕获task里面的异常
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception!{ex.StackTrace}");
            }

            try
            {
                // 对于有返回值的Task, 我们接收了它的返回值就不需要再调用Wait方法了
                // GetName 里面的异常我们也可以捕获到
                var task2 = Task.Run(() => { return TaskExecptionGetName(); });
                var name = task2.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception!{ex.StackTrace}");
            }


        }
        static void TaskExecptionGo() { throw null; }
        static string TaskExecptionGetName() { throw null; }

        #endregion



        #region 一个小例子认识async & await

        [TestMethod]
        public void AsyncAndAwaitDemo1()
        {
            AsyncAndAwaitTest(); // 这个方法其实是多余的, 本来可以直接写下面的方法
            // await AsyncAndAwaitGetName() 
            // 但是由于控制台的入口方法不支持async,所有我们在入口方法里面不能 用 await

            Console.WriteLine("Current Thread Id :{0}", Thread.CurrentThread.ManagedThreadId);

            Thread.Sleep(600); //不这样，是显示不了后面的消息的。因为另一个线程还没有执行完就关掉了主线程了。
        }

        static async Task AsyncAndAwaitTest()
        {
            // 方法打上async关键字，就可以用await调用同样打上async的方法
            // await 后面的方法将在另外一个线程中执行
            await AsyncAndAwaitGetName();
        }

        static async Task AsyncAndAwaitGetName()
        {
            // Delay 方法来自于.net 4.5
            await Task.Delay(500);  // 返回值前面加 async 之后，方法里面就可以用await了
            Console.WriteLine("Current Thread Id :{0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("In antoher thread.....");
        }



        #endregion


        #region await 的原形


        //NOTE: 　await 不会开启新的线程，当前线程会一直往下走直到遇到真正的Async方法（比如说HttpClient.GetStringAsync），这个方法的内部会用Task.Run或者Task.Factory.StartNew 去开启线程。
        //也就是如果方法不是.NET为我们提供的Async方法，我们需要自己创建Task，才会真正的去创建线程。


        [TestMethod]
        public void AwaitDemo1()
        {
            Console.WriteLine("Main Thread Id: {0}\r\n", Thread.CurrentThread.ManagedThreadId);
            AwaitTest();
            //Console.ReadLine();
            Thread.Sleep(3000);
        }

        static async Task AwaitTest()
        {
            Console.WriteLine("Before calling GetName, Thread Id: {0}\r\n", Thread.CurrentThread.ManagedThreadId);
            var name = AwaitGetName();   //我们这里没有用 await,所以下面的代码可以继续执行
            // 但是如果上面是 await AwaitGetName()，下面的代码就不会立即执行，输出结果就不一样了。
            Console.WriteLine("End calling GetName.\r\n");
            Console.WriteLine("Get result from GetName: {0}", await name);
        }

        static async Task<string> AwaitGetName()
        {
            // 这里还是主线程
            Console.WriteLine("Before calling Task.Run, current thread Id is: {0}", Thread.CurrentThread.ManagedThreadId);
            return await Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("'GetName' Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
                return "Jesse";
            });
        }



        #endregion


        #region 只有async方法在调用前才能加await么？

        [TestMethod]
        public void AsyncVoidDemo1()
        {
            AsyncVoidTest();
            Console.WriteLine("我执行完成了。");

            Thread.Sleep(2000);
        }

        static async void AsyncVoidTest()
        {
            Task<string> task = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return "Hello World";
            });
            string str = await task;  //1 秒之后才会执行这里
            Console.WriteLine(str);
        }

        [TestMethod]
        public async Task AsyncVoidDemo2()
        {
            await AsyncVoidTest2();
            Console.WriteLine("我执行完成了。");

            Thread.Sleep(2000);
        }

        //注意：这种在VS中测试执行是失败的。
        [TestMethod]
        public async void AsyncVoidDemo3()
        {
            await AsyncVoidTest2();
            Console.WriteLine("我执行完成了。");

            Thread.Sleep(2000);
        }

        static async Task AsyncVoidTest2()
        {
            Task<string> task = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return "Hello World";
            });
            string str = await task;  //1 秒之后才会执行这里
            Console.WriteLine(str);
        }


        //答案很明显：await并不是针对于async的方法，而是针对async方法所返回给我们的Task，
        //这也是为什么所有的async方法都必须返回给我们Task。
        //所以我们同样可以在Task前面也加上await关键字，这样做实际上是告诉编译器我需要等这个Task的返回值或者等这个Task执行完毕之后才能继续往下走。

        #endregion



        #region 不用await关键字，如何确认Task执行完毕了？

        [TestMethod]
        public void CheckFinishOfTaskDemo1()
        {
            var task = Task.Run(() =>
            {
                return CheckFinishOfTaskGetName();
            });

            //等待其它线程完成
            task.GetAwaiter().OnCompleted(() =>
            {
                // 1 秒之后才会执行这里
                var name = task.Result;
                Console.WriteLine($"[{DateTime.Now:mm:ss ffffff}]My name is: " + name);
            });

            //这儿会立即执行
            Console.WriteLine($"[{DateTime.Now:mm:ss ffffff}]主线程执行完毕");
            Thread.Sleep(2000);
        }

        static string CheckFinishOfTaskGetName()
        {
            Console.WriteLine($"[{DateTime.Now:mm:ss ffffff}]另外一个线程在获取名称");
            Thread.Sleep(1000);
            return "Jesse";
        }


        #endregion


        #region Task如何让主线程挂起等待？

        [TestMethod]
        public void WaitInMainThreadDemo1()
        {
            var task = Task.Run(() =>
            {
                return WaitInMainThreadGetName();
            });

            var name = task.GetAwaiter().GetResult(); //GetResult 是阻塞式执行。
            Console.WriteLine("My name is:{0}", name);

            Console.WriteLine("主线程执行完毕");
            Thread.Sleep(3000);
        }

        static string WaitInMainThreadGetName()
        {
            Console.WriteLine("另外一个线程在获取名称");
            Thread.Sleep(2000);
            return "Jesse";
        }

        //Task.GetAwait()方法会给我们返回一个awaitable的对象，通过调用这个对象的GetResult方法就会挂起主线程，当然也不是所有的情况都会挂起。
        //还记得我们Task的特性么？ 在一开始的时候就启动了另一个线程去执行这个Task，当我们调用它的结果的时候如果这个Task已经执行完毕，主线程是不用等待可以直接拿其结果的，如果没有执行完毕那主线程就得挂起等待了。



        #endregion



        #region await 实质是在调用awaitable对象的GetResult方法

        [TestMethod]
        public async Task AwaitDemo()
        {
            Task<string> task = Task.Run(() =>
            {
                Console.WriteLine("另一个线程在运行！");  // 这句话只会被执行一次
                Thread.Sleep(2000);
                return "Hello World";
            });

            // 这里主线程会挂起等待，直到task执行完毕我们拿到返回结果
            var result = task.GetAwaiter().GetResult();
            // 这里不会挂起等待，因为task已经执行完了，我们可以直接拿到结果
            Console.WriteLine(result + $"{DateTime.Now:mm:ss ffffff}");
            var result2 = await task;
            Console.WriteLine(result2 + $"{DateTime.Now:mm:ss ffffff}");
        }



        #endregion




    }



}
