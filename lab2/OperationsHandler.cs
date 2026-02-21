namespace lab2;

internal struct StringAndCallback
{
    public string? value;
    public Action? callback;
}

public class OperationsHandler
{
    private StudentManager studentManager;

    private readonly Dictionary<string, StringAndCallback> operations;

    public OperationsHandler(StudentManager manager)
    {
        studentManager = manager;

        operations = new Dictionary<string, StringAndCallback> 
        {
            {
                "__help",
                new StringAndCallback
                {
                    value = "",
                    callback = PrintHelp
                }
            },
            {
                "0",
                new StringAndCallback
                {
                    value = "Выйти",
                    callback = () => { Environment.Exit(0); }
                }
            },
            {
                "1",
                new StringAndCallback
                {
                    value = "Вывести список всех групп",
                    callback = PrintGroups
                }
            },
            {
                "2",
                new StringAndCallback
                {
                    value = "Вывести список всех студентов",
                    callback = PrintStudents
                }
            },
            {
                "3",
                new StringAndCallback
                {
                    value = "Вывести учебные результаты студентов",
                    callback = PrintStudyInfo
                }
            },
            {
                "4",
                new StringAndCallback
                {
                    value = "Добавить студента",
                    callback = AddStudent
                }
            },
            {
                "5",
                new StringAndCallback
                {
                    value = "Удалить студента",
                    callback = RemoveStudent
                }
            },
            {
                "6",
                new StringAndCallback
                {
                    value = "Экспортировать в файл",
                    callback = Export
                }
            },
            {
                "7",
                new StringAndCallback
                {
                    value = "Импортировать из файла",
                    callback = Import
                }
            },
        };

        PrintHelp();
    }

    public void TryInvokeOperation(string operation)
    {
        if (operation is null)
            return;

        if (!operations.ContainsKey(operation))
            return;

        var callback = operations[operation].callback;
        if (callback is not null)
            callback();
    }

    public void PrintHelp()
    {
        Console.Clear();
        Console.WriteLine("Доступные операции:");
        foreach (var pair in operations)
        {
            var operation = pair.Key;
            var text = pair.Value.value;

            if (operation == "__help")
                continue;

            Console.WriteLine($"\t{operation}: {text}");
        }
        Console.Write("Ввод: ");
    }

    public void PrintGroups()
    {
        Console.Clear();
        studentManager.PrintGroups();

        Wait();
    }

    public void PrintStudents()
    {
        Console.Clear();
        studentManager.PrintStudents();

        Wait();
    }

    public void PrintStudyInfo()
    {
        Console.Clear();
        studentManager.PrintStudyInfo();

        Wait();
    }

    public void AddStudent()
    {
        Console.Clear();
        Console.WriteLine("Введите имя");
        var name = Console.ReadLine();

        Console.WriteLine("Введите фамилию");
        var surname = Console.ReadLine();

        Console.WriteLine("Введите возраст");
        var age = Console.ReadLine();
        uint ageUint = 0;
        if (age is not null)
            ageUint = uint.Parse(age);

        Console.WriteLine("Введите группу");
        var group = Console.ReadLine();

        Console.WriteLine("Введите текущие оценки в формате '5, 2, 3, 4'");
        var grades = Console.ReadLine();
        List<int> gradesList = (grades ?? "")
            .Split(',')
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(int.Parse)
            .ToList();

        if (name is not null && surname is not null && group is not null)
            studentManager.AddStudent(name, surname, ageUint, group, gradesList);

        Wait();
    }

    public void RemoveStudent()
    {
        Console.Clear();
        Console.WriteLine("Введите номер студента");
        var id = Console.ReadLine();
        if (id is null)
            return;

        uint idUint = uint.Parse(id);
        studentManager.RemoveStudent(idUint);

        Wait();
    }

    public void Export()
    {
        Console.Clear();
        Console.Write("Введите путь, куда экспортировать: ");
        var path = Console.ReadLine();
        if (path is null)
            return;

        studentManager.ExportStudents(path);

        Wait();
    }

    public void Import()
    {
        Console.Clear();
        Console.Write("Введите путь, откуда импортировать: ");
        var path = Console.ReadLine();
        if (path is null)
            return;

        studentManager.ImportStudents(path);

        Wait();
    }

    private void Wait()
    {
        Console.WriteLine("Нажмите ENTER, чтобы продолжить");
        Console.ReadLine();
        PrintHelp();
    }
}
