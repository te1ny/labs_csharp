namespace lab2;

using System.Text.Json;

public class Person
{
    private string? name;
    private string? surname;
    private uint    age;

    public string? Name    { get => name;    set => name    = value; }
    public string? Surname { get => surname; set => surname = value; }
    public uint    Age     { get => age;     set => age     = value; }
}

public class Student : Person
{
    private string?     group;
    private List<int>?  grades;

    public string?     Group  { get => group;  set => group  = value; }
    public List<int>?  Grades { get => grades; set => grades = value; }

    public string Stringify()
    {
        string gradesStringify = "null";

        if (Grades is not null)
        {
            gradesStringify = "[" + string.Join(", ", Grades) + "]";
        }

        string result = 
        $$"""
            {
                "name": "{{Name}}",
                "surname": "{{Surname}}",
                "age": {{Age}},
                "group": "{{Group}}",
                "grades": {{gradesStringify}}
            }
        """;

        return result;
    }

    public void Unstringify(JsonElement element)
    {
        Name    = element.GetProperty("name").GetString();
        Surname = element.GetProperty("surname").GetString();
        Age     = element.GetProperty("age").GetUInt32();
        Group   = element.GetProperty("group").GetString();
        Grades  = element.GetProperty("grades").EnumerateArray().Select(x => x.GetInt32()).ToList();
    }
}
