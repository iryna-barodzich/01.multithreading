/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static Random random = new Random();

        static int Min = 0;
        static int Max = 100;
        static int ArraySize = 10;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            RunChainOfTasks();

            Console.ReadLine();
        }

        static void RunChainOfTasks()
        {
            Task<int[]> taskOne = Task.Run(() => GetArrayOfIntegers());

            Task<int[]> taskTwo = taskOne.ContinueWith(intArray => MultiplyArray(intArray.Result));

            Task<int[]> taskThree = taskTwo.ContinueWith(intArray => SortArray(intArray.Result));

            Task<int> taskFour = taskThree.ContinueWith(intArray => GetAverage(intArray.Result));
        }

        static int[] GetArrayOfIntegers()
        {
            Console.WriteLine("Task 1: Get Array Of Integers");
            int[] intArray = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                intArray[i] = random.Next(Min, Max);
                Console.WriteLine($"[{i}] = {intArray[i]}");
            }
            return intArray;
        }

        static int[] MultiplyArray(int[] intArray)
        {
            Console.WriteLine("Task 2: Multiply Array");
            int multiplier = random.Next(Min, Max);
            Console.WriteLine($"Multiplier: {multiplier}");

            for (int i = 0; i < ArraySize; i++)
            {
                intArray[i] = intArray[i] * multiplier;
                Console.WriteLine($"[{i}] = {intArray[i]}");
            }
            return intArray;
        }

        static int[] SortArray(int[] intArray)
        {
            Console.WriteLine("Task 3: Sort Array By Ascending");
            Array.Sort(intArray);

            for (int i = 0; i < ArraySize; i++)
            {
                Console.WriteLine($"[{i}] = {intArray[i]}");
            }

            return intArray;
        }

        static int GetAverage(int[] intArray)
        {
            Console.WriteLine("Task 4: Get Average");
            int sum = 0;
            for (int i = 0; i < ArraySize; i++)
            {
                sum += intArray[i];
            }

            int average = sum / ArraySize;

            Console.WriteLine(average);
            return average;
        }
    }
}
