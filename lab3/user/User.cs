namespace Lab3.Commands;

using System.Reflection;
using System.Text;

public partial class User
{
    public string      Nickname  { get; private set; } = "unknown";
    public UserRole    Role      { get; private set; } = UserRole.Unauthorized;
    public double      Balance   { get; private set; } = 0;
    public List<Order> Orders    { get; private set; } = [];

    public static bool CanExecute(MethodInfo methodInfo, UserRole userRole)
    {
        var attribute = methodInfo.GetCustomAttribute<AvailableToRolesAttribute>();
        if (attribute == null)
            return false; 

        return attribute.Roles.Any(role => role == userRole);
    }

    public static string GetRoleColor(UserRole role)
    {
        return role switch
        {
            UserRole.Unauthorized => Color.Gray,
            UserRole.Customer     => Color.White,
            UserRole.Admin        => Color.Red,
            _                     => Color.Clear
        };
    }

    private static string[]? FindUserIn(string nickname, string path)
    {
        return File.ReadLines(path)
                   .Select(line => line.Split(';'))
                   .FirstOrDefault(parts => parts[0].Equals(nickname, StringComparison.Ordinal));
    }

    public string GetOrderLine()
    {
        var sb = new StringBuilder();
        sb.Append(Nickname).Append(";");

        for (int i = 0; i < Orders.Count; i++)
        {
            var o = Orders[i];
            sb.Append($"{o.Details.Book.Name},{o.Details.Book.Author},{o.Details.Book.Year},{o.Details.Book.Price},{o.Details.Count},{o.Date},{o.IsSuccessful}");

            if (i < Orders.Count - 1)
                sb.Append("|");
        }

        return sb.ToString();
    }

    private void LoadOrders(string orderLine)
    {
        var orders = orderLine.Split("|");
        foreach (var order in orders)
        {
            var orderParts = order.Split(",");
            var o = new Order {
                Details = new BookPair {
                    Book = new Book {
                        Name = orderParts[0],
                        Author = orderParts[1],
                        Year = orderParts[2],
                        Price = double.Parse(orderParts[3]),
                    },
                    Count = int.Parse(orderParts[4])
                },
                Date = DateTime.Parse(orderParts[5]),
                IsSuccessful = bool.Parse(orderParts[6])
            };
            Orders.Add(o);
        }
    }

    private void SaveOrders()
    {
        string[] allLines = File.ReadAllLines(Data.OrdersFilePath);

        for (int i = 0; i < allLines.Length; i++)
        {
            if (allLines[i].StartsWith(Nickname))
            {
                allLines[i] = GetOrderLine();
                break;
            }
        }

        File.WriteAllLines(Data.OrdersFilePath, allLines);
    }

    public void AddOrder(Order order)
    {
        Orders.Add(order);
        Balance -= order.TotalPrice;
        SaveOrders();
        SaveBalance();
    }

    public void SaveBalance()
    {
        string[] allLines = File.ReadAllLines(Data.BalanceFilePath);

        for (int i = 0; i < allLines.Length; i++)
        {
            if (allLines[i].StartsWith(Nickname))
            {
                allLines[i] = $"{Nickname};{Balance}";
                break;
            }
        }

        File.WriteAllLines(Data.BalanceFilePath, allLines);
    }
}

public enum UserRole
{
    Unauthorized,
    Customer,
    Admin,
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class AvailableToRolesAttribute(params UserRole[] roles) : Attribute
{
    public UserRole[] Roles { get; } = roles;
}
