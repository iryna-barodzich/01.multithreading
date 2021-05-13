/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static List<int> items = new List<int>();
        const int numberOfItems = 10;
        static ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();
        static AutoResetEvent autoResetEvent1 = new AutoResetEvent(false);
        static AutoResetEvent autoResetEvent2 = new AutoResetEvent(false);
        static object locker = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var task1 = Task.Factory.StartNew(Write);
            var task2 = Task.Factory.StartNew(Read);

            Task.WaitAll(task1, task2);

            Console.ReadLine();
        }

        static void Write()
        {
            for (int i = 1; i < 10; ++i)
            {
                lock(locker)
                {
                    items.Add(i);
                }
                autoResetEvent1.Set();
                autoResetEvent2.WaitOne();
            }
        }

        static void Read()
        {
            for (int i = 1; i < 10; ++i)
            {
                autoResetEvent1.WaitOne();
                lock (locker)
                {
                    Console.WriteLine(string.Join(",", items));
                }
                autoResetEvent2.Set();

            }
        }
    }
}
