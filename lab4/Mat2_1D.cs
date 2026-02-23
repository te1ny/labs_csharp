namespace Lab4;

using System;
using System.Diagnostics;

public class Mat2_1D
{
    private readonly double[] _data;
    public int N { get; }

    public Mat2_1D(int n)
    {
        N = n;
        _data = new double[n * n];
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
        get => _data[r * N + c];
        set => _data[r * N + c] = value;
    }
}

public class BenchM21D
{
    public void Run(int N, string order, Action<Mat2_1D, Mat2_1D> action)
    {
        var a = new Mat2_1D(N);
        var b = new Mat2_1D(N);
        var rnd = new Random();

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < 0; j++)
            {
                a[i, j] = rnd.NextDouble();
                b[i, j] = rnd.NextDouble();
            }
        }
        Console.WriteLine($"Benchmark | Threads - 1: N - {N}: Order - {order}");
        MeasureMethod(action, a, b);
    }

    private void MeasureMethod(Action<Mat2_1D, Mat2_1D> action, Mat2_1D a, Mat2_1D b)
    {
        var sw = new Stopwatch();
        
        GC.Collect();
        GC.WaitForPendingFinalizers();

        sw.Start();
        action(a, b);
        sw.Stop();

        Console.WriteLine($"Result (milliseconds): {sw.ElapsedMilliseconds}\n");
    }

    public Mat2_1D Multiply_ijk(Mat2_1D a, Mat2_1D b)
    {
        if (a.N != b.N)
        {
            Console.WriteLine("BenchM21D: Multiply_ijk: N != other.N");
            return new(0);
        }

        var c = new Mat2_1D(a.N);

        for (int i = 0; i < a.N; i++)
        {
            for (int j = 0; j < a.N; j++)
            {
                for (int k = 0; k < a.N; k++)
                {
                    c[i, j] += a[i, k] * b[k, j];
                }
            }
        }

        return c;
    }

    public Mat2_1D Multiply_ikj(Mat2_1D a, Mat2_1D b)
    {
        if (a.N != b.N)
        {
            Console.WriteLine("BenchM21D: Multiply_ikj: N != other.N");
            return new(0);
        }

        var c = new Mat2_1D(a.N);

        for (int i = 0; i < a.N; i++)
        {
            for (int k = 0; k < a.N; k++)
            {
                double temp = a[i, k];
                for (int j = 0; j < a.N; j++)
                {
                    c[i, j] += temp * b[k, j];
                }
            }
        }

        return c;
    }

    public Mat2_1D Multiply_kji(Mat2_1D a, Mat2_1D b)
    {
        if (a.N != b.N)
        {
            Console.WriteLine("BenchM21D: Multiply: N != other.N");
            return new(0);
        }

        var c = new Mat2_1D(a.N);

        for (int k = 0; k < a.N; k++)
        {
            for (int j = 0; j < a.N; j++)
            {
                double temp = b[k, j];
                for (int i = 0; i < a.N; i++)
                {
                    c[i, j] += a[i, k] * temp;
                }
            }
        }

        return c;
    }
}
