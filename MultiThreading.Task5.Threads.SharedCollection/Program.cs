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
        static ConcurrentBag<int> bag = new ConcurrentBag<int>();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            object locker = new object();

            Task task1 = Task.Factory.StartNew(() =>
            {
                for (int i = 1; i < 10; ++i)
                {
                    bag.Add(i);
                    Task.Run(() => Read()).Wait();
                }
            });

            task1.Wait();

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
