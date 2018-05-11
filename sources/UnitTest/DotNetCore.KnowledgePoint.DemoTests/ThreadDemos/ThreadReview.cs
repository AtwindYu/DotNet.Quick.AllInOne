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



        #region ����

        [TestMethod]
        public void ThreadDemo1()
        {
            new Thread(ThreadGo).Start();  // .NET 1.0��ʼ���е�
            Task.Factory.StartNew(ThreadGo); // .NET 4.0 ������ TPL
            Task.Run(new Action(ThreadGo)); // .NET 4.5 ������һ��Run�ķ���

            //����Thread��ʵ��֮����Ҫ�ֶ���������Start��������������
            //���Ƕ���Task��˵��StartNew��Run��ͬʱ���Ȼᴴ���µ��̣߳����һ�������������

            // Thread.Sleep(500); //Ϊ�˿�������ĵȴ�
        }

        public static void ThreadGo()
        {
            Thread.Sleep(200);
            Console.WriteLine("������һ���߳�");
        }


        #endregion



        #region �̳߳�


        [TestMethod]
        public void ThreadPoolDemo1()
        {
            Console.WriteLine("�������̣߳�Thread Id {0}", Thread.CurrentThread.ManagedThreadId);
            ThreadPool.QueueUserWorkItem(ThreadPoolGo);

            //�̵߳Ĵ����ǱȽ�ռ����Դ��һ�����飬.NET Ϊ�����ṩ���̳߳����������Ǵ����͹����̡߳�
            //Task��Ĭ�ϻ�ֱ��ʹ���̳߳أ�����Thread���ᡣ
            //������ǲ�ʹ��Task���������̳߳صĻ�������ʹ��ThreadPool�ࡣ

            //Console.ReadLine();
        }

        public static void ThreadPoolGo(object data)
        {
            Console.WriteLine("������һ���߳�:Thread Id {0}", Thread.CurrentThread.ManagedThreadId);
        }



        #endregion



        #region �������

        [TestMethod]
        public void InputParamDemo1()
        {
            new Thread(InputParamGo).Start("arg1"); // û������ί��֮ǰ������ֻ����������һ��object�Ĳ���

            new Thread(delegate ()
            {  // ��������ί��֮��...
                InputParamGoGoGo("arg1", "arg2", "arg3");
            });

            new Thread(() =>
            {  // ��Ȼ,���� Lambada
                InputParamGoGoGo("arg1", "arg2", "arg3");
            }).Start();

            Task.Run(() =>
            {  // Task����ô��Ҳ����Ϊ����Lambdaѽ��
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



        #region ����ֵ

        [TestMethod]
        public void ReuntnValueDemo1()
        {
            // GetDayOfThisWeek ����������һ���߳���
            var dayName = Task.Run<DateTime>(() => { return DateTime.Now; });
            Console.WriteLine("�����ǣ�{0}", dayName.Result);

            //Thead�ǲ��ܷ���ֵ�ģ�������Ϊ���߼���Task��ȻҪ�ֲ�һ��������ܡ�
        }


        #endregion



        #region ��������


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
                _isDone = true; // �ڶ����߳�����ʱ�򣬾Ͳ�����ִ����(Ҳ���Ǿ��Եģ�ȡ���ڼ������CPU�����Լ���ʱ���������)
                Console.WriteLine("Done");
            }
        }

        //�߳�֮�����ͨ��static�������������ݡ�

        #endregion



        #region �̰߳�ȫ


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
                Console.WriteLine("Done"); // �²�������ᱻִ�м��Σ�
                _isDone = true;
            }
        }

        //���������������һֱ��������������������õĻ����ͻ��н��ˡ�
        //��Ϊ��һ���̻߳�û�����ü���_isDone���ó�true���ڶ����߳̾ͽ����ˣ����ⲻ��������Ҫ�Ľ����
        //�ڶ���߳��£�����������ǵ�Ԥ�ڽ����������̲߳���ȫ��


        #endregion


        #region ��

        //Ҫ����������������⣬���Ǿ�Ҫ�õ�����
        //���������ж�ռ�������������Լ���д���ȣ���������ͼ���ʾһ�¶�ռ����

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
                    Console.WriteLine("Done"); // �²�������ᱻִ�м��Σ�
                    _isDone = true;
                }
            }
        }

        //�����Ǽ�����֮�󣬱���ס�Ĵ�����ͬһ��ʱ����ֻ����һ���̷߳��ʣ��������̻߳ᱻ������ֻ�еȵ���������ͷ�֮���������̲߳���ִ�б���ס�Ĵ��롣

        #endregion


        #region Semaphore �ź���


        //�����Կ��ƶ�ĳһ�δ�����߶�ĳ����Դ���ʵ��̵߳������������������֮���������߳̾͵õȴ���ֻ�е��������߳��ͷ���֮��������̲߳��ܷ��ʡ�
        //������������ƵĹ��ܣ�ֻ�������Ƕ�ռ�ģ�������һ���������߳�ͬʱ���ʡ�

        static SemaphoreSlim _sem = new SemaphoreSlim(3);    // ����������ͬʱ���ʵ��߳�������3
        [TestMethod]
        public void SemaphoreDemo1()
        {
            for (int i = 1; i <= 5; i++) new Thread(SemaphoreEnter).Start(i);
            //Console.ReadLine();

            Thread.Sleep(8000);

        }

        static void SemaphoreEnter(object id)
        {
            Console.WriteLine(id + $" ��ʼ�Ŷ�... [{DateTime.Now:HH:mm:ss ffffff}]");
            _sem.Wait();
            Console.WriteLine(id + $" ��ʼִ�У� [{DateTime.Now:HH:mm:ss ffffff}]");
            Thread.Sleep(1000 * (int)id);
            Console.WriteLine(id + $" ִ����ϣ��뿪��[{DateTime.Now:HH:mm:ss ffffff}]");
            _sem.Release();
        }


        #endregion



        #region �쳣����   --�������̵߳��쳣�����߳̿��Բ���ô��

        [TestMethod]
        public void ThreadExceptionDemo1()
        {
            try
            {
                new Thread(ThreadExceptionGo).Start();
            }
            catch (Exception ex)
            {
                // �����߳�������쳣�������������ǲ��񲻵��ġ�
                Console.WriteLine("Exception!");
            }
            Thread.Sleep(200);
        }
        static void ThreadExceptionGo() { throw null; }

        //��ô�����˵�Task�أ�
        [TestMethod]
        public void TaskExecptionDemo1()
        {
            try
            {
                var task = Task.Run(() => { TaskExecptionGo(); });
                task.Wait();  // �ڵ�������仰֮�����̲߳��ܲ���task������쳣
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception!{ex.StackTrace}");
            }

            try
            {
                // �����з���ֵ��Task, ���ǽ��������ķ���ֵ�Ͳ���Ҫ�ٵ���Wait������
                // GetName ������쳣����Ҳ���Բ���
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



        #region һ��С������ʶasync & await

        [TestMethod]
        public void AsyncAndAwaitDemo1()
        {
            AsyncAndAwaitTest(); // ���������ʵ�Ƕ����, ��������ֱ��д����ķ���
            // await AsyncAndAwaitGetName() 
            // �������ڿ���̨����ڷ�����֧��async,������������ڷ������治�� �� await

            Console.WriteLine("Current Thread Id :{0}", Thread.CurrentThread.ManagedThreadId);

            Thread.Sleep(600); //������������ʾ���˺������Ϣ�ġ���Ϊ��һ���̻߳�û��ִ����͹ص������߳��ˡ�
        }

        static async Task AsyncAndAwaitTest()
        {
            // ��������async�ؼ��֣��Ϳ�����await����ͬ������async�ķ���
            // await ����ķ�����������һ���߳���ִ��
            await AsyncAndAwaitGetName();
        }

        static async Task AsyncAndAwaitGetName()
        {
            // Delay ����������.net 4.5
            await Task.Delay(500);  // ����ֵǰ��� async ֮�󣬷�������Ϳ�����await��
            Console.WriteLine("Current Thread Id :{0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("In antoher thread.....");
        }



        #endregion


        #region await ��ԭ��


        //NOTE: ��await ���Ὺ���µ��̣߳���ǰ�̻߳�һֱ������ֱ������������Async����������˵HttpClient.GetStringAsync��������������ڲ�����Task.Run����Task.Factory.StartNew ȥ�����̡߳�
        //Ҳ���������������.NETΪ�����ṩ��Async������������Ҫ�Լ�����Task���Ż�������ȥ�����̡߳�


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
            var name = AwaitGetName();   //��������û���� await,��������Ĵ�����Լ���ִ��
            // ������������� await AwaitGetName()������Ĵ���Ͳ�������ִ�У��������Ͳ�һ���ˡ�
            Console.WriteLine("End calling GetName.\r\n");
            Console.WriteLine("Get result from GetName: {0}", await name);
        }

        static async Task<string> AwaitGetName()
        {
            // ���ﻹ�����߳�
            Console.WriteLine("Before calling Task.Run, current thread Id is: {0}", Thread.CurrentThread.ManagedThreadId);
            return await Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("'GetName' Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
                return "Jesse";
            });
        }



        #endregion


        #region ֻ��async�����ڵ���ǰ���ܼ�awaitô��

        [TestMethod]
        public void AsyncVoidDemo1()
        {
            AsyncVoidTest();
            Console.WriteLine("��ִ������ˡ�");

            Thread.Sleep(2000);
        }

        static async void AsyncVoidTest()
        {
            Task<string> task = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return "Hello World";
            });
            string str = await task;  //1 ��֮��Ż�ִ������
            Console.WriteLine(str);
        }

        [TestMethod]
        public async Task AsyncVoidDemo2()
        {
            await AsyncVoidTest2();
            Console.WriteLine("��ִ������ˡ�");

            Thread.Sleep(2000);
        }

        //ע�⣺������VS�в���ִ����ʧ�ܵġ�
        [TestMethod]
        public async void AsyncVoidDemo3()
        {
            await AsyncVoidTest2();
            Console.WriteLine("��ִ������ˡ�");

            Thread.Sleep(2000);
        }

        static async Task AsyncVoidTest2()
        {
            Task<string> task = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return "Hello World";
            });
            string str = await task;  //1 ��֮��Ż�ִ������
            Console.WriteLine(str);
        }


        //�𰸺����ԣ�await�����������async�ķ������������async���������ظ����ǵ�Task��
        //��Ҳ��Ϊʲô���е�async���������뷵�ظ�����Task��
        //��������ͬ��������Taskǰ��Ҳ����await�ؼ��֣�������ʵ�����Ǹ��߱���������Ҫ�����Task�ķ���ֵ���ߵ����Taskִ�����֮����ܼ��������ߡ�

        #endregion



        #region ����await�ؼ��֣����ȷ��Taskִ������ˣ�

        [TestMethod]
        public void CheckFinishOfTaskDemo1()
        {
            var task = Task.Run(() =>
            {
                return CheckFinishOfTaskGetName();
            });

            //�ȴ������߳����
            task.GetAwaiter().OnCompleted(() =>
            {
                // 1 ��֮��Ż�ִ������
                var name = task.Result;
                Console.WriteLine($"[{DateTime.Now:mm:ss ffffff}]My name is: " + name);
            });

            //���������ִ��
            Console.WriteLine($"[{DateTime.Now:mm:ss ffffff}]���߳�ִ�����");
            Thread.Sleep(2000);
        }

        static string CheckFinishOfTaskGetName()
        {
            Console.WriteLine($"[{DateTime.Now:mm:ss ffffff}]����һ���߳��ڻ�ȡ����");
            Thread.Sleep(1000);
            return "Jesse";
        }


        #endregion


        #region Task��������̹߳���ȴ���

        [TestMethod]
        public void WaitInMainThreadDemo1()
        {
            var task = Task.Run(() =>
            {
                return WaitInMainThreadGetName();
            });

            var name = task.GetAwaiter().GetResult(); //GetResult ������ʽִ�С�
            Console.WriteLine("My name is:{0}", name);

            Console.WriteLine("���߳�ִ�����");
            Thread.Sleep(3000);
        }

        static string WaitInMainThreadGetName()
        {
            Console.WriteLine("����һ���߳��ڻ�ȡ����");
            Thread.Sleep(2000);
            return "Jesse";
        }

        //Task.GetAwait()����������Ƿ���һ��awaitable�Ķ���ͨ��������������GetResult�����ͻ�������̣߳���ȻҲ�������е�����������
        //���ǵ�����Task������ô�� ��һ��ʼ��ʱ�����������һ���߳�ȥִ�����Task�������ǵ������Ľ����ʱ��������Task�Ѿ�ִ����ϣ����߳��ǲ��õȴ�����ֱ���������ģ����û��ִ����������߳̾͵ù���ȴ��ˡ�



        #endregion



        #region await ʵ�����ڵ���awaitable�����GetResult����

        [TestMethod]
        public async Task AwaitDemo()
        {
            Task<string> task = Task.Run(() =>
            {
                Console.WriteLine("��һ���߳������У�");  // ��仰ֻ�ᱻִ��һ��
                Thread.Sleep(2000);
                return "Hello World";
            });

            // �������̻߳����ȴ���ֱ��taskִ����������õ����ؽ��
            var result = task.GetAwaiter().GetResult();
            // ���ﲻ�����ȴ�����Ϊtask�Ѿ�ִ�����ˣ����ǿ���ֱ���õ����
            Console.WriteLine(result + $"{DateTime.Now:mm:ss ffffff}");
            var result2 = await task;
            Console.WriteLine(result2 + $"{DateTime.Now:mm:ss ffffff}");
        }



        #endregion




    }



}
