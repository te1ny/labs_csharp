namespace Lab3;

using Commands;

public class Service(string[] args)
{
    private void PrintPrompt()
    {
        string color = User.GetRoleColor(Controller.Instance.User.Role);
        Console.Write($"{color}{Controller.Instance.User.Nickname}{Color.Clear} $ ");
    }

    private void HandleHelp(string[] args)
    {
        int index;
        index = Array.IndexOf(args, "--help");
        if (index != -1)
        {
            string help =
            """
            usage:
              optional:
                --help                  - print this message
                --version               - print application version
                --users-path <string>   - path to file with data about users               (default 'data/users.txt')
                --orders-path <string>  - path to file with data about users orders        (default 'data/orders.txt')
                --balance-path <string> - path to file with data about users balances      (default 'data/balance.txt')
                --books-path <string>   - path to file with data about books               (default 'data/books.txt')
                --enable-validator      - flag, when specified, checks for data corruption
            """;
            Console.WriteLine(help);
            Environment.Exit(0);
        }
    }

    private void HandleVersion(string[] args)
    {
        int index;
        index = Array.IndexOf(args, "--version");
        if (index != -1)
        {
            string version = "version: 0.0.0";
            Console.WriteLine(version);
            Environment.Exit(0);
        }
    }

    public void Run()
    {
        HandleHelp(args);
        HandleVersion(args);

        var processor = new CommandProcessor();

        Console.Clear();
        Data.LoadFromArgs(args);
        while (true)
        {
            PrintPrompt();

            string? command = Console.ReadLine();
            if (command is null)
                continue;

            processor.TryExecuteCommand(command);
        }
    }
}
