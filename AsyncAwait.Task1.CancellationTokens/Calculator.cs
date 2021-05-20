using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens
{
    static class Calculator
    {
        private static long sum = 0;
        private static object sync = new object();
        // todo: change this method to support cancellation token
        public async static Task<long> Calculate(int n, CancellationToken token)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < n; i++)
            {
                if(token.IsCancellationRequested)
                {
                    return sum;
                }
                tasks.Add(ProcessSum(i));
            }

            await Task.WhenAll(tasks.ToArray());

            return sum;

        }

        private static Task ProcessSum(int i)
        {

            return Task.Run(() =>
            {
                lock(sync)
                {
                    Thread.Sleep(10);
                    sum = sum + (i + 1);
                }
            });
        }
    }
}
