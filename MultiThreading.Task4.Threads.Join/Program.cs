/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private const int NumberOfThreads = 10;
        static Semaphore semaphore = new Semaphore(NumberOfThreads, NumberOfThreads);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            Console.WriteLine("Create Threads With Join");
            ProcessNumberWithJoin(NumberOfThreads + 1);

            Console.WriteLine("Create ThreadPool With Semaphore");
            CreateThreadPoolWithSemaphore(NumberOfThreads + 1);

            Console.ReadLine();
        }

        static void ProcessNumberWithJoin(object number)
        {
            var newNumber = (int)number - 1;
            if(newNumber > 0)
            {
                Thread thread = new Thread(ProcessNumberWithJoin);
                thread.Start(newNumber);
                Console.WriteLine($"{newNumber} thread works");
                thread.Join();
                Console.WriteLine($"{newNumber} thread task completed");
            }
        }

        static void CreateThreadPoolWithSemaphore(int number)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessNumberWithSemaphore), number);
        }

        static void ProcessNumberWithSemaphore(object number)
        {
            var newNumber = (int)number - 1;
            if (newNumber > 0)
            {
                semaphore.WaitOne();
                Thread thread = new Thread(ProcessNumberWithSemaphore);
                thread.Start(newNumber);
                Console.WriteLine($"{newNumber} thread works");
                semaphore.Release();
                Console.WriteLine($"Semaphore released from {newNumber} thread ");
            }
        }
    }
}
