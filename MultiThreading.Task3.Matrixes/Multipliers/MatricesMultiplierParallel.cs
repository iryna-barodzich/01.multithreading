using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            var resultMatrix = new Matrix(m1.RowCount, m2.ColCount);

            List<int> m1RowCountList = Enumerable.Range(0, (int)m1.RowCount).ToList();
            Parallel.ForEach(m1RowCountList, i =>
            {
                List<int> m2ColCountList = Enumerable.Range(0, (int)m2.ColCount).ToList();
                Parallel.ForEach(m2ColCountList, j =>
                {
                    long sum = 0;

                    List<int> m1ColCountList = Enumerable.Range(0, (int)m1.ColCount).ToList();
                    Parallel.ForEach(m1ColCountList, k =>
                    {
                        sum += m1.GetElement(i, k) * m2.GetElement(k, j);
                    });

                    resultMatrix.SetElement(i, j, sum);
                });
            });

            return resultMatrix;
        }
    }
}
