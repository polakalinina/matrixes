using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Task1
{
    public class MatrixProcessor
    {
        private readonly int[,] _firstMatrix;
        private readonly int[,] _secondMatrix;
        private readonly int[,] _result;

        private readonly Random _random = new Random();

        public MatrixProcessor(int matrixSize)
        {
            _firstMatrix = new int[matrixSize, matrixSize];
            _secondMatrix = new int[matrixSize, matrixSize];
            _result = new int[matrixSize, matrixSize];

            for(var i = 0; i < matrixSize; i++)
            {
                for(var j = 0; j < matrixSize; j++)
                {
                    _firstMatrix[i, j] = _random.Next(-10, 10);
                    _secondMatrix[i, j] = _random.Next(-10, 10);
                    _result[i, j] = 0;
                }
            }
        }

        public void SumSequentially(int matrixSize)
        {
            for (var i = 0; i < matrixSize; i++)
            {
                for(var j = 0; j < matrixSize; j++)
                {
                    _result[i, j] = StrongFunction(_firstMatrix[i, j] + _secondMatrix[i, j]);
                }
            }
        }

        public int[] SumParallel(int threadsCount, int matrixSize)
        {
            var times = new int[threadsCount];
            
            Parallel.For(
                0, threadsCount, k =>
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    
                    for (var i = k; i < matrixSize; i += threadsCount)
                    {
                        for (var j = 0; j < matrixSize; j++)
                        {
                            _result[i, j] = StrongFunction(_firstMatrix[i, j] + _secondMatrix[i, j]);
                        }
                    }
                    
                    watch.Stop();
                    times[k] = (int)watch.ElapsedMilliseconds;
                });

            return times;
        }

        private static int StrongFunction(long number)
        { 
            return (int) Math.Pow(number, 2);
        }
    }
}