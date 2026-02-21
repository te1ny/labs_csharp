namespace lab2;

using System.Text.Json;

public class StudentManager
{
    private List<Student> students = [];

    public List<Student> Students { get => students; private set => students = value; }
    
    public string GetStudentCommonInfo(Student student)
    {
        return $"Студент: {student?.Surname} {student?.Name}, Группа: {student?.Group}";
    }

    public string GetStudentStudyInfo(Student student)
    {
        double avg = 0;
        foreach (var grade in student.Grades)
            avg += grade;
        avg /= student.Grades.Count;

        string gradesStringify = "[" + string.Join(", ", student.Grades) + "]";
        string examResult = avg >= 3.0f ? "Сдал" : "Не сдал";

        return $"Студент: {student?.Surname} {student?.Name}, Среднее: {avg}, Экзамен: {examResult}, Оценки: {gradesStringify}";
    }

    public void PrintStudents()
    {
        Console.WriteLine("Все студенты:");
        for (int i = 0; i < students.Count; i++)
            Console.WriteLine($"\tНомер: {i}, " + GetStudentCommonInfo(students[i]));
    }

    public void PrintGroups()
    {
        var groups = new HashSet<string>();
        foreach (var student in students)
            groups.Add(student.Group);
        
        Console.WriteLine("Группы:");
        foreach (var group in groups)
            Console.WriteLine("\t" + group);
    }

    public void PrintStudyInfo()
    {
        Console.WriteLine("Все студенты:");
        for (int i = 0; i < students.Count; i++)
            Console.WriteLine($"\tНомер: {i}, " + GetStudentStudyInfo(students[i]));
    }
    public void AddStudent(string name, string surname, uint age, string group, List<int> grades)
    {
        var student = new Student {
            Name    = name,
            Surname = surname,
            Age     = age,
            Group   = group,
            Grades  = grades
        };

        students.Add(student);
    }

    public void RemoveStudent(uint id)
    {
        students.RemoveAt((int)id);
    }

    public void ExportStudents(string path)
    {
        File.WriteAllText(path, "[\n");
        for (int i = 0; i < students.Count; i++)
            File.AppendAllText(path, students[i].Stringify() + (i + 1 == students.Count ? "" : ","));
        File.AppendAllText(path, "\n]");
    }

    public void ImportStudents(string path)
    {
        students.Clear();
        var document = JsonDocument.Parse(File.ReadAllText(path));
        var root     = document.RootElement;
        foreach (var studentElement in root.EnumerateArray())
        {
            var student = new Student();
            student.Unstringify(studentElement);
            students.Add(student);
        }
    }
}
