namespace Lab3.Commands;

using System.Reflection;

public class Help
{
    private void HelpAbout(Type type)
    {
        Console.WriteLine("usage:");

        var className = Utility.ToSnakeCase(type.Name);
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var method in methods)
        {
            if (method.IsSpecialName || !Commands.User.CanExecute(method, Controller.Instance.User.Role))
                continue;

            var methodName = Utility.ToSnakeCase(method.Name);
            var args = method.GetParameters();
            
            if (className == "single")
                className = "";
            {}
            var result = $"  {className} {methodName}";
            foreach (var arg in args)
            {
                string? argName     = arg.Name;
                string  argTypeName = arg.ParameterType.Name;
                if (argName is not null)
                {
                    result += $" <{argName}:{argTypeName}>";
                }
                else
                {
                    result += $" <:{argTypeName}>";
                }
            }
            Console.WriteLine(result);
        }
    }

    [AvailableToRoles(UserRole.Unauthorized, UserRole.Customer, UserRole.Admin)]
    public void Single()
    {
        HelpAbout(typeof(Single));
    }

    [AvailableToRoles(UserRole.Unauthorized, UserRole.Customer, UserRole.Admin)]
    public void User()
    {
        HelpAbout(typeof(User));
    }

    [AvailableToRoles(UserRole.Unauthorized, UserRole.Customer, UserRole.Admin)]
    public void Catalog()
    {
        HelpAbout(typeof(Catalog));
    }
}
