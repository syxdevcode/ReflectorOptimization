using ReflectorOptimization.Common;
using System;
using System.Diagnostics;
using System.Reflection;

namespace ReflectorOptimization.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Type t = typeof(Person);
            MethodInfo methodInfo = t.GetMethod("Say");
            Person person = new Person();
            string word = "hello";
            Person p = null;

            object[] param = new object[] { word, p, 3 };
            int testCount = 100000;//测试次数

            #region 传统方式反射

            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                for (int i = 0; i < testCount; i++)
                {
                    methodInfo.Invoke(person, param);
                }
                watch.Stop();
                Console.WriteLine(testCount.ToString() + " times invoked by Reflection:" + watch.ElapsedMilliseconds + "ms");
            }
            catch (System.Exception ex)
            { }
            #endregion 传统方式反射

            #region 快速反射

            try
            {
                Stopwatch watch1 = new Stopwatch();
                FastMethodInvoker.FastInvokeHandler fastInvoker = FastMethodInvoker.GetMethodInvoker(methodInfo);
                watch1.Start();
                for (int i = 0; i < testCount; i++)
                {
                    fastInvoker(person, param);
                }
                watch1.Stop();
                Console.WriteLine(testCount.ToString() + " times invoked by FastInvoke: " + watch1.ElapsedMilliseconds + "ms");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("快速反射 错误:" + ex.Message);
            }

            #endregion 快速反射

            #region 直接调用

            try
            {
                Stopwatch watch2 = new Stopwatch();
                watch2.Start();
                for (int i = 0; i < testCount; i++)
                {
                    person.Say(ref word, out p, 3);
                }
                watch2.Stop();
                Console.WriteLine(testCount.ToString() + " times invoked by DirectCall: " + watch2.ElapsedMilliseconds + "ms");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("直接调用 错误:" + ex.Message);
            }

            #endregion 直接调用

            Console.ReadKey();
        }
    }

    public class Person
    {
        public void Say(ref string word, out Person p, int avi)
        {
            word = "ttt" + avi.ToString();
            p = new Person();

            //throw new System.Exception("出错了哦");
        }
    }
}