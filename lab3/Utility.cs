namespace Lab3;

using System.Text;

public static class Utility
{
    public static string ToSnakeCase(string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return str;

        var sb = new StringBuilder(str.Length + 5);
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            if (char.IsUpper(c))
            {
                if (i > 0 && str[i - 1] != '_') 
                    sb.Append('_');
                
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public static string ToCamelCase(string str)
    {
        string pascal = ToPascalCase(str);
        if (string.IsNullOrEmpty(pascal)) return pascal;

        var sb = new StringBuilder(pascal);
        sb[0] = char.ToLowerInvariant(sb[0]);
        
        return sb.ToString();
    }

    public static string ToPascalCase(string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return str;

        var sb = new StringBuilder(str.Length);
        bool capitalizeNext = true;

        foreach (char c in str)
        {
            if (!char.IsLetterOrDigit(c))
            {
                capitalizeNext = true;
                continue;
            }

            sb.Append(capitalizeNext ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
            capitalizeNext = false;
        }

        return sb.ToString();
    }
}
