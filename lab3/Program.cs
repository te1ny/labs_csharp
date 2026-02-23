namespace Lab3;

class Program
{
    static void Main(string[] args)
    {
        var service = new Service(args);
        service.Run();
    }
}
