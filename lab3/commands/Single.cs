namespace Lab3.Commands;

public class Single
{
    [AvailableToRoles(UserRole.Unauthorized, UserRole.Customer, UserRole.Admin)]
    public void Help()
    {
        Console.WriteLine("usage:");

        var properties = typeof(Controller).GetProperties();
        foreach (var property in properties)
        {
            string name = Utility.ToSnakeCase(property.Name);

            if (name == "help" || name == "instance") 
                continue;

            Console.WriteLine($"  help {name}");
        }
    }

    [AvailableToRoles(UserRole.Unauthorized, UserRole.Customer, UserRole.Admin)]
    public void Clear()
    {
        Console.Clear();
    }

    [AvailableToRoles(UserRole.Unauthorized, UserRole.Customer, UserRole.Admin)]
    public void Exit()
    {
        Environment.Exit(0);
    }
}
