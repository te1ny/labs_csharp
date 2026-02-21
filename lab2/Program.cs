namespace lab2;

class Program
{
    static void Main(string[] args)
    {
        var manager           = new StudentManager();
        var operationsHandler = new OperationsHandler(manager);

        while (true)
        {
            var operation = Console.ReadLine();
            if (operation is null)
                continue;

            operationsHandler.TryInvokeOperation(operation);
        }
    }
}
