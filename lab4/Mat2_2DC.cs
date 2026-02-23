namespace Lab4;

using System;
using System.Diagnostics;

public class Mat2_2DC
{
    private readonly double[,] _data;
    public int N { get; }

    public Mat2_2DC(int n)
    {
        N = n;
        _data = new double[n, n];
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                this[i, j] = 0;
            }
        }
    }

    public double this[int r, int c]
    {
        get => _data[r, c];
        set => _data[r, c] = value;
    }
}

public class BenchM22DC
{
    public void Run(int N, int k, string order, Action<Mat2_2DC, Mat2_2DC, int> action)
    {
        var a = new Mat2_2DC(N);
        var b = new Mat2_2DC(N);
        var rnd = new Random();

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < 0; j++)
            {
                a[i, j] = rnd.NextDouble();
                b[i, j] = rnd.NextDouble();
            }
        }
        Console.WriteLine($"Benchmark | Threads - {k}: N - {N}: Order - {order}");
        MeasureMethod(action, a, b, k);
    }

    private void MeasureMethod(Action<Mat2_2DC, Mat2_2DC, int> action, Mat2_2DC a, Mat2_2DC b, int k)
    {
        var sw = new Stopwatch();
        
        GC.Collect();
        GC.WaitForPendingFinalizers();

        sw.Start();
        action(a, b, k);
        sw.Stop();

        Console.WriteLine($"Result (milliseconds): {sw.ElapsedMilliseconds}\n");
    }

    public Mat2_2DC Multiply_ijk(Mat2_2DC a, Mat2_2DC b, int kThreads)
    {
        if (a.N != b.N)
        {
            Console.WriteLine("BenchM22DC: Multiply_ijk: N != other.N");
            return new(0);
        }

        var c = new Mat2_2DC(a.N);
        var options = new ParallelOptions { MaxDegreeOfParallelism = kThreads };

        Parallel.For(0, a.N, options, i =>
        {
            for (int j = 0; j < a.N; j++)
            {
                double sum = 0;
                for (int k = 0; k < a.N; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                c[i, j] = sum;
            }
        });

        return c;
    }

    public Mat2_2DC Multiply_ikj(Mat2_2DC a, Mat2_2DC b, int kThreads)
    {
        if (a.N != b.N)
        {
            Console.WriteLine("BenchM22DC: Multiply_ikj: N != other.N");
            return new(0);
        }

        var c = new Mat2_2DC(a.N);
        var options = new ParallelOptions { MaxDegreeOfParallelism = kThreads };

        Parallel.For(0, a.N, options, i =>
        {
            for (int k = 0; k < a.N; k++)
            {
                double temp = a[i, k]; 
                for (int j = 0; j < a.N; j++)
                {
                    c[i, j] += temp * b[k, j];
                }
            }
        });

        return c;
    }

    public Mat2_2DC Multiply_kji(Mat2_2DC a, Mat2_2DC b, int kThreads)
    {
        if (a.N != b.N)
        {
            Console.WriteLine("BenchM22DC: Multiply: N != other.N");
            return new(0);
        }

        var c = new Mat2_2DC(a.N);
        var options = new ParallelOptions { MaxDegreeOfParallelism = kThreads };

        Parallel.For(0, a.N, options, j =>
        {
            for (int k = 0; k < a.N; k++)
            {
                double temp = b[k, j];
                for (int i = 0; i < a.N; i++)
                {
                    c[i, j] += a[i, k] * temp;
                }
            }
        });

        return c;
    }
}
