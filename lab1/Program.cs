namespace lab1;

class Program
{
    static void Main(string[] args)
    {
        var car      = new Car {
            Brand         = "Toyota",
            MaxSpeed      = 180,
            NumberOfDoors = 5
        };

        var bicycle  = new Bicycle {
            Brand    = "Stern",
            MaxSpeed = 35,
            HasRing  = true
        };

        var airplane = new Airplane {
            Brand    = "Boeing",
            MaxSpeed = 840,
            WindSpan = 10
        };

        var transports = new List<Transport>{ car, bicycle, airplane };

        foreach (var transport in transports)
        {
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine(transport.GetInfo());
            if (transport is IDrivable drivable)
            {
                Console.WriteLine("\nInvoke Drive():");
                drivable.Drive();
            }
        }
        Console.WriteLine("----------------------------------------------------------------");
    }
}
