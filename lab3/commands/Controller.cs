namespace Lab3.Commands;

public class Controller
{
// Singleton
    private static readonly Lazy<Controller> _instance = new(() => new Controller());
    public static Controller Instance { get => _instance.Value; } 
    private Controller() {}

// Commands
    public Help    Help    { get; private set; } = new();
    public Single  Single  { get; private set; } = new();

    public User    User    { get; private set; } = new();
    public Catalog Catalog { get; private set; } = new();
}
