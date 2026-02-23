namespace Lab3.Commands;

using BCrypt.Net;

public partial class User
{
    [AvailableToRoles(UserRole.Unauthorized, UserRole.Customer, UserRole.Admin)]
    public string Register(string nickname, string password)
    {
        if (nickname == "unknown")
            return "nope :D (this is the system name tsssss)";

        var userLine = FindUserIn(nickname, Data.UsersFilePath);
        if (userLine is not null)
            return "already registered";

        var role = UserRole.Customer;

        var fileInfo = new FileInfo(Data.UsersFilePath);
        if (fileInfo.Length == 0)
            role = UserRole.Admin;

        var hash = BCrypt.HashPassword(password);

        File.AppendAllLines(Data.UsersFilePath,   [ $"{nickname};{hash};{role}" ]);
        File.AppendAllLines(Data.OrdersFilePath,  [ $"{nickname};" ]);
        File.AppendAllLines(Data.BalanceFilePath, [ $"{nickname};0" ]);

        return "";
    }

    [AvailableToRoles(UserRole.Unauthorized, UserRole.Customer, UserRole.Admin)]
    public string Login(string nickname, string password)
    {
        if (nickname == "unknown")
            return "nope :D (this is the system name)";

        if (nickname == Nickname)
            return "already logged in";

        var userLine = FindUserIn(nickname, Data.UsersFilePath);
        if (userLine is null)
            return "the username is not registered";

        var hash = userLine[1];
        if (!BCrypt.Verify(password, hash))
            return "incorrect password";

        Logout();

        var ordersLine = FindUserIn(nickname, Data.OrdersFilePath);
        if (ordersLine is null)
            return "data corruption found: user orders data is missing";

        var balanceLine = FindUserIn(nickname, Data.BalanceFilePath);
        if (balanceLine is null)
            return "data corruption found: user balance data is missing";

        Nickname = nickname;
        Role = (UserRole)Enum.Parse(typeof(UserRole), userLine[2]);
        Balance = double.Parse(balanceLine[1]);
        if (!string.IsNullOrWhiteSpace(ordersLine[1]))
        {
            LoadOrders(ordersLine[1]);
        }

        return "";
    }

    [AvailableToRoles(UserRole.Unauthorized, UserRole.Customer, UserRole.Admin)]
    public void Logout()
    {
        Nickname = "unknown";
        Role     = UserRole.Unauthorized;
        Balance  = 0;
        Orders   = [];
    }

    [AvailableToRoles(UserRole.Customer)]
    public void GetBalance()
    {
        Console.WriteLine($"{Balance}");
    }

    // its not to be here
    [AvailableToRoles(UserRole.Customer)]
    public void AddBalance(double value)
    {
        Balance += Math.Abs(value);
        SaveBalance();
    }

    [AvailableToRoles(UserRole.Customer)]
    public void GetOrders()
    {
        Console.WriteLine($"orders:");
        foreach (var order in Orders)
        {
            var name         = order.Details.Book.Name;
            var author       = order.Details.Book.Author;
            var year         = order.Details.Book.Year;
            var price        = order.Details.Book.Price;
            var count        = (int)(order.TotalPrice / price); // possible divide by zero ;)
            var date         = order.Date;
            var isSuccessful = order.IsSuccessful;
            var totalPrice   = order.TotalPrice;

            string result = 
            $"""
                date: {date}
                book: {name}, {author}, {year}
                price per unit: {price}, count: {count}
                total_price: {totalPrice}, is_successful: {isSuccessful}
            """;

            Console.WriteLine(result);
        }
    }
}
