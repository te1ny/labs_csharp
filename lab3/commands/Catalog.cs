namespace Lab3.Commands;

public partial class Catalog
{
    [AvailableToRoles(UserRole.Unauthorized, UserRole.Customer, UserRole.Admin)]
    public void GetBooks()
    {
        Load(); // lazy load

        Console.WriteLine("available books:");
        int i = 0;
        foreach (var bookPair in books)
        {
            string result =
            $"""
                {i}: {bookPair.Book.Name}, {bookPair.Book.Author}, {bookPair.Book.Year}, price per unit: {bookPair.Book.Price}, quantity: {bookPair.Count}
            """;
            Console.WriteLine(result);
        }
    }

    [AvailableToRoles(UserRole.Admin)]
    public string AddBooks(int index, int count)
    {
        Load(); // lazy load

        index = Math.Abs(index);
        count = Math.Abs(count);

        if (index >= books.Count)
            return "out of range";
        books[index] = books[index] with { Count = books[index].Count + count };
        SaveBooks();
        return "";
    }

    [AvailableToRoles(UserRole.Admin)]
    public string RemoveBook(int index, int count)
    {
        Load(); // lazy load

        index = Math.Abs(index);
        count = Math.Abs(count);

        if (index >= books.Count)
            return "out of range";
        books[index] = books[index] with { Count = books[index].Count - count };
        SaveBooks();
        return "";
    }

    [AvailableToRoles(UserRole.Admin)]
    public void AddNewBooks(string name, string author, string year, double price, int count)
    {
        Load(); // lazy load

        var book = new Book {
            Name = name,
            Author = author,
            Year = year,
            Price = price
        };

        var bookPair = new BookPair {
            Book = book,
            Count = Math.Abs(count)
        };

        books.Add(bookPair);

        SaveBooks();
    }

    [AvailableToRoles(UserRole.Customer)]
    public string PlaceOrder(int index, int count)
    {
        Load(); // lazy load

        index = Math.Abs(index);
        count = Math.Abs(count);

        if (index >= books.Count)
            return "out of range";

        var bookPair = books[index];
        if (count > bookPair.Count)
            return "not enough books";

        var totalPrice = bookPair.Book.Price * count;
        var user = Controller.Instance.User;

        var order        = new Order {
            Details      = new BookPair {
                Book     = bookPair.Book,
                Count    = count
            },
            Date         = DateTime.Now,
            IsSuccessful = user.Balance >= totalPrice
        };

        user.AddOrder(order);
        
        if (order.IsSuccessful)
            RemoveBook(index, count);
        else
            return "not enough money";

        return "";
    }
}
