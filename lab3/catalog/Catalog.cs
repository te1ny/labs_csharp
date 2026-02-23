namespace Lab3.Commands;

public partial class Catalog
{
    private List<BookPair> books = []; // bullshit
    private bool           loaded = false;

    private void Load()
    {
        if (loaded)
            return;

        var lines = File.ReadLines(Data.BooksFilePath);
        foreach (var line in lines)
        {
            var lineParts = line.Split(";");

            var book   = new Book {
                Name   = lineParts[0],
                Author = lineParts[1],
                Year   = lineParts[2],
                Price  = double.Parse(lineParts[3])
            };

            var count = int.Parse(lineParts[4]);

            books.Add(new BookPair{ Book = book, Count = count });
        }

        loaded = true;
    }

    private void SaveBooks()
    {
        var lines = books.Select(p => $"{p.Book.Name};{p.Book.Author};{p.Book.Year};{p.Book.Price};{p.Count}");
        File.WriteAllLines(Data.BooksFilePath, lines);
    }
}

public struct Book
{
    public string Name   { get; set; }
    public string Author { get; set; }
    public string Year   { get; set; }
    public double Price  { get; set; }
}

public struct BookPair
{
    public Book Book  { get; set; }
    public int  Count { get; set => field = Math.Max(value, 0);}
}

public struct Order
{
    public BookPair  Details      { get; set; }
    public DateTime  Date         { get; set; }
    public bool      IsSuccessful { get; set; }

    public readonly double TotalPrice => Details.Book.Price * Details.Count;
}
