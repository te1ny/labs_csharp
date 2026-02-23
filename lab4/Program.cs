using Lab4;

namespace lab4;

class Program
{
    static void Main(string[] args)
    {
        var bm21d = new BenchM21D();
        var bm22d = new BenchM22D();
        var bm21dc = new BenchM21DC();
        var bm22dc = new BenchM22DC();


        Console.WriteLine("--------------- Mat2_1D ---------------");
        bm21d.Run(1000, "IJK", (a, b) => bm21d.Multiply_ijk(a, b));
        bm21d.Run(1000, "IKJ", (a, b) => bm21d.Multiply_ikj(a, b));
        bm21d.Run(1000, "KJI", (a, b) => bm21d.Multiply_kji(a, b));

        Console.WriteLine("--------------- Mat2_2D ---------------");
        bm22d.Run(1000, "IJK", (a, b) => bm22d.Multiply_ijk(a, b));
        bm22d.Run(1000, "IKJ", (a, b) => bm22d.Multiply_ikj(a, b));
        bm22d.Run(1000, "KJI", (a, b) => bm22d.Multiply_kji(a, b));


        Console.WriteLine("--------------- Mat2_1DC (2) ---------------");
        bm21dc.Run(1000, 2, "IJK", (a, b, k) => bm21dc.Multiply_ijk(a, b, k));
        bm21dc.Run(1000, 2, "IKJ", (a, b, k) => bm21dc.Multiply_ikj(a, b, k));
        bm21dc.Run(1000, 2, "KJI", (a, b, k) => bm21dc.Multiply_kji(a, b, k));

        Console.WriteLine("--------------- Mat2_2DC (2) ---------------");
        bm22dc.Run(1000, 2, "IJK", (a, b, k) => bm22dc.Multiply_ijk(a, b, k));
        bm22dc.Run(1000, 2, "IKJ", (a, b, k) => bm22dc.Multiply_ikj(a, b, k));
        bm22dc.Run(1000, 2, "KJI", (a, b, k) => bm22dc.Multiply_kji(a, b, k));

        
        Console.WriteLine("--------------- Mat2_1DC (4) ---------------");
        bm21dc.Run(1000, 4, "IJK", (a, b, k) => bm21dc.Multiply_ijk(a, b, k));
        bm21dc.Run(1000, 4, "IKJ", (a, b, k) => bm21dc.Multiply_ikj(a, b, k));
        bm21dc.Run(1000, 4, "KJI", (a, b, k) => bm21dc.Multiply_kji(a, b, k));

        Console.WriteLine("--------------- Mat2_2DC (4) ---------------");
        bm22dc.Run(1000, 4, "IJK", (a, b, k) => bm22dc.Multiply_ijk(a, b, k));
        bm22dc.Run(1000, 4, "IKJ", (a, b, k) => bm22dc.Multiply_ikj(a, b, k));
        bm22dc.Run(1000, 4, "KJI", (a, b, k) => bm22dc.Multiply_kji(a, b, k));


        Console.WriteLine("--------------- Mat2_1DC (8) ---------------");
        bm21dc.Run(1000, 8, "IJK", (a, b, k) => bm21dc.Multiply_ijk(a, b, k));
        bm21dc.Run(1000, 8, "IKJ", (a, b, k) => bm21dc.Multiply_ikj(a, b, k));
        bm21dc.Run(1000, 8, "KJI", (a, b, k) => bm21dc.Multiply_kji(a, b, k));

        Console.WriteLine("--------------- Mat2_2DC (8) ---------------");
        bm22dc.Run(1000, 8, "IJK", (a, b, k) => bm22dc.Multiply_ijk(a, b, k));
        bm22dc.Run(1000, 8, "IKJ", (a, b, k) => bm22dc.Multiply_ikj(a, b, k));
        bm22dc.Run(1000, 8, "KJI", (a, b, k) => bm22dc.Multiply_kji(a, b, k));


        Console.WriteLine("--------------- Mat2_1DC (16) ---------------");
        bm21dc.Run(1000, 16, "IJK", (a, b, k) => bm21dc.Multiply_ijk(a, b, k));
        bm21dc.Run(1000, 16, "IKJ", (a, b, k) => bm21dc.Multiply_ikj(a, b, k));
        bm21dc.Run(1000, 16, "KJI", (a, b, k) => bm21dc.Multiply_kji(a, b, k));

        Console.WriteLine("--------------- Mat2_2DC (16) ---------------");
        bm22dc.Run(1000, 16, "IJK", (a, b, k) => bm22dc.Multiply_ijk(a, b, k));
        bm22dc.Run(1000, 16, "IKJ", (a, b, k) => bm22dc.Multiply_ikj(a, b, k));
        bm22dc.Run(1000, 16, "KJI", (a, b, k) => bm22dc.Multiply_kji(a, b, k));


        Console.WriteLine("--------------- Mat2_1DC (32) ---------------");
        bm21dc.Run(1000, 32, "IJK", (a, b, k) => bm21dc.Multiply_ijk(a, b, k));
        bm21dc.Run(1000, 32, "IKJ", (a, b, k) => bm21dc.Multiply_ikj(a, b, k));
        bm21dc.Run(1000, 32, "KJI", (a, b, k) => bm21dc.Multiply_kji(a, b, k));

        Console.WriteLine("--------------- Mat2_2DC (32) ---------------");
        bm22dc.Run(1000, 32, "IJK", (a, b, k) => bm22dc.Multiply_ijk(a, b, k));
        bm22dc.Run(1000, 32, "IKJ", (a, b, k) => bm22dc.Multiply_ikj(a, b, k));
        bm22dc.Run(1000, 32, "KJI", (a, b, k) => bm22dc.Multiply_kji(a, b, k));
    }
}
