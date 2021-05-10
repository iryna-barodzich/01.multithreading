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
        static EventWaitHandle firstEvent = new AutoResetEvent(false);
        static EventWaitHandle secondEvent = new AutoResetEvent(false);
        static ConcurrentBag<int> bag = new ConcurrentBag<int>();
        static int CollectionSize = 10;

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            /*var readingTask = Task.Factory.StartNew(Read);

            Task.Factory.StartNew(() =>
            {
                for(int i = 0; i < CollectionSize; i++)
                {
                    list.Add(i);
                    readingTask.Wait();
                }
            });*/


            object locker = new object();

            Task t1 = Task.Factory.StartNew(() =>
            {
                for (int i = 1; i < 10; ++i)
                {
                    bag.Add(i);
                    //firstEvent.Set();
                    Task.Run(() => Read()).Wait();
                }
            });

            Task t2 = Task.Factory.StartNew(() =>
            {

              //  firstEvent.WaitOne();
              //  Read();
            });
            Task.WaitAll(t1, t2);

            Console.ReadLine();
        }

        static void Read()
        {
            Console.WriteLine("Reading list:");
            foreach (var item in bag)
            {
                Console.WriteLine(item);
            }

        }
    }
}
