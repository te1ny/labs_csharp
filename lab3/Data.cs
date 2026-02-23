namespace Lab3;

public static class Data
{
    // unix compatible only - bad
    public static string UsersFilePath    { get; private set; } = "data/users.txt";
    public static string BooksFilePath    { get; private set; } = "data/books.txt";
    public static string OrdersFilePath   { get; private set; } = "data/orders.txt";
    public static string BalanceFilePath  { get; private set; } = "data/balance.txt";

    public static bool   ValidatorEnabled { get; private set; } = false;

    public static void LoadFromArgs(string[] args)
    {
        int argsCount = args.Count();
        int index;

        // nickname;hash;role
        index = Array.IndexOf(args, "--users-path");
        if (index != -1 && index + 1 < argsCount)
        { 
            UsersFilePath = args[index + 1];
        }
        CheckPath(UsersFilePath);

        // name;author;year;price;count
        index = Array.IndexOf(args, "--books-path");
        if (index != -1 && index + 1 < argsCount)
        { 
            BooksFilePath = args[index + 1];
        }
        CheckPath(BooksFilePath);

        // nickname;order, order, ..., order
        index = Array.IndexOf(args, "--orders-path");
        if (index != -1 && index + 1 < argsCount)
        { 
            OrdersFilePath = args[index + 1];
        }
        CheckPath(OrdersFilePath);

        // nickname;balance
        index = Array.IndexOf(args, "--balance-path");
        if (index != -1 && index + 1 < argsCount)
        { 
            BalanceFilePath = args[index + 1];
        }
        CheckPath(BalanceFilePath);
        
        index = Array.IndexOf(args, "--enable-validator");
        if (index != -1)
        { 
            ValidatorEnabled = true;
            CheckDataCorruption();
        }
    }

    private static void CheckPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return;

        string? directory = Path.GetDirectoryName(path);

        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        // throw an exception 
        // if the file is not created 
        // or cannot be opened, 
        // or if there are no permissions
        using var fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    private static void CheckDataCorruption()
    {
        Console.BackgroundColor = ConsoleColor.Red;

        var errorCount = 0;

// USERS
        List<string> nicknamesInUsers = [];

        var userLines = File.ReadLines(UsersFilePath);
        var userLinesCount = userLines.Count();

        for (int i = 0; i < userLinesCount; i++)
        {
            var userParts = userLines.ElementAt(i).Split(";");
            if (userParts.Count() != 3)
            {
                errorCount++;
                Console.WriteLine($"data corruption found: file {UsersFilePath}: invalid parts count: line {i + 1}");
            }
            if (userParts.Count() >= 1 && userParts[0] != null)
                nicknamesInUsers.Add(userParts[0]);
        }

// ORDERS
        var ordersLines = File.ReadLines(OrdersFilePath);
        var ordersLinesCount = ordersLines.Count();

        if (ordersLinesCount < userLinesCount)
        {
            Console.WriteLine($"data corruption found: file {OrdersFilePath}: orders.count < users.count");
            errorCount++;
        }

        List<string> nicknamesInOrders = [];
        bool sortedOrders = true;
        for (int i = 0; i < ordersLinesCount; i++)
        {
            var ordersParts = ordersLines.ElementAt(i).Split(";");
            if (ordersParts.Count() != 2)
            {
                errorCount++;
                Console.WriteLine($"data corruption found: file {OrdersFilePath}: invalid parts count: line {i + 1}");
            }
            if (ordersParts.Count() >= 1 && ordersParts[0] != null)
            {
                nicknamesInOrders.Add(ordersParts[0]);
                if (i < nicknamesInUsers.Count && ordersParts[0] != nicknamesInUsers[i])
                    sortedOrders = false;
            }
        }

        if (!sortedOrders)
        {
            Console.WriteLine($"data corruption found: file {OrdersFilePath}: orders entries do not correspond to user entries in order");
            errorCount++;
        }

        var missingNicksOrders = nicknamesInUsers.Except(nicknamesInOrders).ToList();
        if (missingNicksOrders.Any())
        {
            var nicks = string.Join(", ", missingNicksOrders.Select(n => string.IsNullOrWhiteSpace(n) ? "<empty>" : n));
            Console.WriteLine($"data corruption found: file {OrdersFilePath}: orders does not contain nicknames from users: " + nicks);
            errorCount++;
        }

// BALANCE
        var balanceLines = File.ReadLines(BalanceFilePath);
        var balanceLinesCount = balanceLines.Count();

        if (balanceLinesCount < userLinesCount)
        {
            Console.WriteLine($"data corruption found: file {BalanceFilePath}: balance.count < users.count");
            errorCount++;
        }

        List<string> nicknamesInBalance = [];
        bool sortedBalance = true;
        for (int i = 0; i < balanceLinesCount; i++)
        {
            var balanceParts = balanceLines.ElementAt(i).Split(";");
            if (balanceParts.Count() != 2)
            {
                errorCount++;
                Console.WriteLine($"data corruption found: file {BalanceFilePath}: invalid parts count: line {i + 1}");
            }
            if (balanceParts.Count() >= 1 && balanceParts[0] != null)
            {
                nicknamesInBalance.Add(balanceParts[0]);
                if (i < nicknamesInUsers.Count && balanceParts[0] != nicknamesInUsers[i])
                    sortedBalance = false;
            }
        }

        if (!sortedBalance)
        {
            Console.WriteLine($"data corruption found: file {BalanceFilePath}: balance entries do not correspond to user entries in order");
            errorCount++;
        }

        var missingNicksBalance = nicknamesInUsers.Except(nicknamesInBalance).ToList();
        if (missingNicksBalance.Any())
        {
            var nicks = string.Join(", ", missingNicksBalance.Select(n => string.IsNullOrWhiteSpace(n) ? "<empty>" : n));
            Console.WriteLine($"data corruption found: file {BalanceFilePath}: balance does not contain nicknames from users: " + nicks);
            errorCount++;
        }

// BOOKS
        var booksLines = File.ReadLines(BooksFilePath);
        var booksLinesCount = booksLines.Count();

        for (int i = 0; i < booksLinesCount; i++)
        {
            var booksParts = booksLines.ElementAt(i).Split(";");
            if (booksParts.Count() != 5)
            {
                errorCount++;
                Console.WriteLine($"data corruption found: file {BooksFilePath}: invalid parts count: line {i + 1}");
            }
        }

        if (errorCount > 0)
            Console.WriteLine($"total count data corruptions errors: {errorCount}");

        if (errorCount > 5)
        {
            string art =
            """
                            ______              
                         .-"      "-.           
                        /            \          
                       |              |         
                       |,  .-.  .-.  ,|         
                       | )(__/  \__)( |         
                       |/     /\     \|         
                       (_     ^^     _)         
                        \__|IIIIII|__/          
                         | \IIIIII/ |           
                         \          /           
                          `--------`            
                                                
            [        YOU ARE ON YOUR OWN       ]
            [             GOOD LUCK            ]
            """;
            Console.WriteLine(art);
        }

        Console.ResetColor();
    }
}

public static class Color
{
    public const string Red   = "\u001b[38;2;255;0;0m";
    public const string White = "\u001b[38;2;255;255;255m";
    public const string Gray  = "\u001b[38;2;128;128;128m";
    public const string Clear = "\u001b[0m";
}
