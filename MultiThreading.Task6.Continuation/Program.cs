/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            TaskA();
            TaskB();
            TaskC();
            TaskD();

            Console.ReadLine();
        }

        static void TaskA()
        {
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            var task = Task.Run(() =>
            {
                DateTime date = DateTime.Now;
                var currentYear = date.Year;

                Console.WriteLine($"Current year: {currentYear}");
                return currentYear;
            });

            task.ContinueWith(
                antecedent =>
                {
                    Console.WriteLine($"Next year is {antecedent.Result + 1}");
                })
                .Wait();

        }

        static void TaskB()
        {
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            var task = Task.Run(() =>
            {
                var r = new Random();
                var b = r.Next(0, 1);
                Console.WriteLine($"Trying divide on {b}.");
                int a = 1 / b;
                Console.WriteLine("Success!");
            });

            var task2 = task.ContinueWith(
                antecedent =>
                {
                    Console.WriteLine("Parent task finished without success.");
                    Console.WriteLine("Cannot divide on zero!");
                }, TaskContinuationOptions.NotOnRanToCompletion);

            try
            {
                task2.Wait();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task task finished without success");
            }

        }

        static void TaskC()
        {
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            var task = Task.Run(() =>
            {
                var r = new Random();
                var b = r.Next(0, 1);
                Console.WriteLine($"Trying divide on {b}.");
                int a = 1 / b;
                Console.WriteLine("Success!");
            });

            var task2 = task.ContinueWith(
                antecedent =>
                {
                    Console.WriteLine("Parent task faulted.");
                    Console.WriteLine("Cannot divide on zero!");
                }, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted);

            try
            {
                task2.Wait();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task faulted");
            }

        }

        static void TaskD()
        {
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            Task task = Task.Run(() =>
            {
                Console.WriteLine("Print numbers from 1 to 100:");
                for (int i = 1; i <= 100; i++)
                {
                    token.ThrowIfCancellationRequested();
                    if (!token.IsCancellationRequested)
                    {
                        Console.WriteLine($"Iteration: {i}");
                        Thread.Sleep(1000);
                    }
                }
            }, token).ContinueWith((t) =>
            {
                Console.WriteLine("Parent task status: " + t.Status);
                Console.WriteLine("You have canceled the task");
            }, 
            CancellationToken.None,
            TaskContinuationOptions.LongRunning,
            TaskScheduler.Default);

            new Task(() =>
            {
                Thread.Sleep(2000);
                cancelTokenSource.Cancel();
            }).Start();
        }

    }
}
