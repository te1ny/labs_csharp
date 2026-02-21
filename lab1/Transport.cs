namespace lab1;

public interface IDrivable
{
    public void Drive();
}

public class Transport
{
    public string? Brand    { get; set; }
    public double  MaxSpeed { get; set; }

    public virtual string GetInfo()
    {
        return "Brand: "    + Brand     + "\n" +
               "MaxSpeed: " + MaxSpeed;
    }
}

public class Car : Transport, IDrivable
{
    public int NumberOfDoors { get; set; }

    public override string GetInfo()
    {
        return "Type: "          + "Car"         + "\n" + 
               "NumberOfDoors: " + NumberOfDoors + "\n" +
               base.GetInfo();
    }

    public void Drive()
    {
        Console.WriteLine("Mega shasechki (Аккуратное вождение)");
    }
}

public class Bicycle : Transport, IDrivable
{
    public bool HasRing { get; set; }

    public override string GetInfo()
    {
        return "Type: "    + "Bicycle" + "\n" + 
               "HasRing: " + HasRing   + "\n" +
               base.GetInfo();
    }

    public void Drive()
    {
        Console.WriteLine("Bonus za bezumniy truk (Спокойный спуск с горы)");
    }
}

public class Airplane : Transport, IDrivable
{
    public double WindSpan { get; set; }

    public override string GetInfo()
    {
        return "Type: "     + "Airplane" + "\n" +
               "WindSpan: " + WindSpan   + "\n" +
               base.GetInfo();
    }

    public void Drive()
    {
        Console.WriteLine("Hmm, two identical towers? (Обычный маршрут Бостон-Лос-Анджелес)");
    }
}
