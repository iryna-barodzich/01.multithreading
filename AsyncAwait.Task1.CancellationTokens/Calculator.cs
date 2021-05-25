using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens
{
    static class Calculator
    {
        public static async Task<long> Calculate(int n, CancellationToken token)
        {
            long sum = 0;

            return await Task.Factory.StartNew(() => {

                for (int i = 0; i < n; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        return sum;
                    }
                    sum = sum + (i + 1);
                    Thread.Sleep(10);
                }
                return sum;
            });
        }
    }
}
