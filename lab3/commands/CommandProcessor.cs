namespace Lab3.Commands;

using System.Reflection;

public class CommandProcessor
{
    public void TryExecuteCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command)) 
            return;

        var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1) 
            TryExecuteSingleCommand(parts[0]);
        else
            TryExecuteBaseCommand(parts);
    }

    public void TryExecuteBaseCommand(string[] parts)
    {
        string   classStr  = parts[0];
        string   methodStr = parts[1];
        string[] argsStr   = parts[2..];

        var controllerType = typeof(Controller);
        var controllerProperties = controllerType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var propertyInfo = controllerProperties.FirstOrDefault(p => p.Name.Equals(Utility.ToPascalCase(classStr),
                                                                                  StringComparison.OrdinalIgnoreCase));

        if (propertyInfo is null)
        {
            Console.WriteLine("command error: invalid command");
            return;
        }

        object? classInstance = propertyInfo.GetValue(Controller.Instance);

        if (classInstance is null)
        {
            Console.WriteLine("command error: class not initialized: its not your fault");
            return;
        }

        var methodInfos = classInstance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
        var methodInfo  = methodInfos.FirstOrDefault(m => m.Name.Equals(Utility.ToPascalCase(methodStr),
                                                                        StringComparison.OrdinalIgnoreCase));

        if (methodInfo is null)
        {
            Console.WriteLine($"command error: invalid arg: {methodStr}");
            return;
        }

        if (!User.CanExecute(methodInfo, Controller.Instance.User.Role))
        {
            Console.WriteLine("access error: not enough rights");
            return;
        }

        var parameters = methodInfo.GetParameters();
        if (parameters.Length != argsStr.Length)
        {
            Console.WriteLine($"command error: '{methodStr}' expects ${parameters.Length} arguments, but only ${argsStr.Length} are provided");
            return;
        }

        object[] methodArgs = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            try
            {
                methodArgs[i] = Convert.ChangeType(argsStr[i], parameters[i].ParameterType);
            }
            catch
            {
                string name = Utility.ToSnakeCase(parameters[i].Name); // bullshit
                string type = parameters[i].ParameterType.Name;
                Console.WriteLine($"command error: arg '{name}' expects type ‘{type}’");
                return; 
            }
        }

        try
        {
            object? result = methodInfo.Invoke(classInstance, methodArgs);

            if (result is string error)
            {
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine($"runtime error: {error}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"exception: {ex.InnerException?.Message ?? ex.Message}");
        }
    }

    public void TryExecuteSingleCommand(string command)
    {
        var methodStr = Utility.ToPascalCase(command);
        var methodInfos = Controller.Instance.Single.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
        var methodInfo = methodInfos.FirstOrDefault(m => m.Name.Equals(methodStr, StringComparison.OrdinalIgnoreCase));

        if (methodInfo is null)
        {
            Console.WriteLine("command error: invalid command");
            return;
        }

        if (!User.CanExecute(methodInfo, Controller.Instance.User.Role))
        {
            Console.WriteLine("access error: not enough rights");
            return;
        }

        try
        {
            object? result = methodInfo.Invoke(Controller.Instance.Single, []);

            if (result is string error)
            {
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine($"command error: {error}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"exception: {ex.InnerException?.Message ?? ex.Message}");
        }
    }
}
