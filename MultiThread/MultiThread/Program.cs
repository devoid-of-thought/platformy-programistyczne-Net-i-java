    using System.Diagnostics;
    using System.Numerics;

    namespace MultiThread;

    class MultiplyMatrix
    {
        public int[,] matrixOne;
        public int[,] matrixTwo;
        public int[,] resultMatrix;
        public int Size;

        public void resizeMatrix(int size)
        {
            Size = size;
            matrixOne = new int[size, size];
            matrixTwo = new int[size, size];
            resultMatrix = new int[size, size];
        }

        public void populateMatrixWithRandomValues()
        {
            Random random = new Random(10);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    matrixOne[i, j] = random.Next(1, 99);
                    matrixTwo[i, j] = random.Next(1, 99);
                }
            }
        }

        public void showMatrix(int[,] matrix)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }

                Console.WriteLine();
            }
        }

        public virtual long multiplyMatrixes(int size)
        {
            return 0;
        }
        public bool checkIfMultipliedCorrectly()
        {
            for(int i =  0; i < Size; i++){
                for (int j = 0; j < Size; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < Size; k++)
                    {
                        sum += matrixOne[i, k] * matrixTwo[k, j];
                        
                    }
                    if (resultMatrix[i, j] != sum)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void tests()
        {
            int[] sizes = {100,300,500,700};
            int threadsUsed = Environment.ProcessorCount;
            foreach (int size in sizes)
            {
                Console.WriteLine("Size of matrix: " + size);
                for (int i = 1; i <= threadsUsed + 4; i++)
                {
                    List<long> results = new List<long>();
                    for (int j = 0; j < 10; j++)
                    {
                        resizeMatrix(size);
                        populateMatrixWithRandomValues();
                        results.Add(multiplyMatrixes(i));
                        if (!checkIfMultipliedCorrectly())
                        {
                            Console.WriteLine("Błąd!!!");
                        }
                    }
                    Console.WriteLine( results.Average());

                }   
            }
        }
    }

    class HighLevel : MultiplyMatrix
    {
        public override long multiplyMatrixes(int maxAmountOfThreads)
        {
            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxAmountOfThreads };
            Stopwatch  stopWatch = new Stopwatch();
            stopWatch.Start();
            Parallel.For(0, Size, parallelOptions,i =>
                {
                    for (int j = 0; j < Size; j++)
                    {
                        int sum = 0;
                        for (int k = 0; k < Size; k++)
                        {
                            sum += matrixOne[i, k] * matrixTwo[k, j];
                        }
                        resultMatrix[i, j] = sum;
                    }
                }
            );
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }

    }

    class LowLevel : MultiplyMatrix
    {
        public override long multiplyMatrixes(int amountOfThreads)
        {
            Stopwatch  stopWatch = new Stopwatch();
            stopWatch.Start();
            Thread[] threads = new Thread[amountOfThreads];
            int rowsPerThread = Size/amountOfThreads;
            int remainingThreads = Size % amountOfThreads;
            int currentRow = 0;
            for (int i = 0; i < amountOfThreads; i++)
            {
                int startRow = currentRow;
                int endRow = currentRow + rowsPerThread;
                if (i < remainingThreads)
                {
                    endRow++;
                }
                threads[i] = new Thread(()=> multiply(startRow, endRow));
                currentRow = endRow;
                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }

        public void multiply(int start, int end)
        {
            for(int i=start; i<end; i++){
                for (int j = 0; j < Size; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < Size; k++)
                    {
                        sum += matrixOne[i, k] * matrixTwo[k, j];
                    }

                    resultMatrix[i, j] = sum;
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            int[] sizes = {100,300,500,700};
            int threadsUsed = Environment.ProcessorCount;
            foreach (int size in sizes)
            {
                HighLevel m = new HighLevel();
                LowLevel m1 = new LowLevel();
                Console.WriteLine("Size of matrix: " + size);
                for (int i = 1; i <= threadsUsed + 4; i++)
                {
                    List<long> results_low = new List<long>();
                    List<long> results_high = new List<long>();

                    for (int j = 0; j < 10; j++)
                    {
                        m.resizeMatrix(size);
                        m.populateMatrixWithRandomValues();
                        results_high.Add(m.multiplyMatrixes(i));
                        if (!m.checkIfMultipliedCorrectly())
                        {
                            Console.WriteLine("Błąd!!!");
                        }
                        m1.resizeMatrix(size);
                        m1.populateMatrixWithRandomValues();
                        results_low.Add(m1.multiplyMatrixes(i));
                        if (!m1.checkIfMultipliedCorrectly())
                        {
                            Console.WriteLine("Błąd!!!");
                        }
                    }
                    Console.WriteLine("Dla Pararell: " + results_high.Average());
                    Console.WriteLine("Dla Threads: " + results_low.Average());
                }   
            }
            
        }   
    }