using System;
using System.Collections;
using System.Collections.Generic;

// 1. Інтерфейси та класи для двигунів
public interface IEngine
{
    string Model { get; set; }
    double Power { get; set; }
    void Start();
}

public interface ICombustionEngine : IEngine
{
    string FuelType { get; set; }
}

public interface IDisplayable
{
    void Show();
}

public class Engine : IEngine, IDisplayable
{
    public string Model { get; set; }
    public double Power { get; set; }

    public Engine(string model, double power)
    {
        Model = model;
        Power = power;
    }

    public virtual void Start()
    {
        Console.WriteLine($"{Model} engine started.");
    }

    public virtual void Show()
    {
        Console.WriteLine($"Engine Model: {Model}, Power: {Power} HP");
    }
}

public class CombustionEngine : Engine, ICombustionEngine
{
    public string FuelType { get; set; }

    public CombustionEngine(string model, double power, string fuelType)
        : base(model, power)
    {
        FuelType = fuelType;
    }

    public override void Show()
    {
        base.Show();
        Console.WriteLine($"Fuel Type: {FuelType}");
    }
}

public class DieselEngine : CombustionEngine
{
    public double FuelConsumption { get; set; }

    public DieselEngine(string model, double power, double consumption)
        : base(model, power, "Diesel")
    {
        FuelConsumption = consumption;
    }

    public override void Show()
    {
        base.Show();
        Console.WriteLine($"Fuel Consumption: {FuelConsumption} L/100km");
    }
}

public class JetEngine : Engine
{
    public double Thrust { get; set; }

    public JetEngine(string model, double power, double thrust)
        : base(model, power)
    {
        Thrust = thrust;
    }

    public override void Show()
    {
        base.Show();
        Console.WriteLine($"Thrust: {Thrust} kN");
    }
}

// 2. Функції для математичних розрахунків
public interface Function : ICloneable, IComparable<Function>
{
    double Calculate(double x);
    string GetInfo(double x);
}

public class Line : Function
{
    public double A { get; set; }
    public double B { get; set; }

    public Line(double a, double b)
    {
        A = a;
        B = b;
    }

    public double Calculate(double x) => A * x + B;

    public string GetInfo(double x) =>
        $"[Line] y = {A}x + {B}; x = {x}; y = {Calculate(x)}";

    public object Clone() => new Line(A, B);

    public int CompareTo(Function other) =>
        Calculate(1).CompareTo(other.Calculate(1));
}

public class Kub : Function
{
    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }

    public Kub(double a, double b, double c)
    {
        A = a;
        B = b;
        C = c;
    }

    public double Calculate(double x) => A * x * x + B * x + C;

    public string GetInfo(double x) =>
        $"[Kub] y = {A}x² + {B}x + {C}; x = {x}; y = {Calculate(x)}";

    public object Clone() => new Kub(A, B, C);

    public int CompareTo(Function other) =>
        Calculate(1).CompareTo(other.Calculate(1));
}

public class Hyperbola : Function
{
    public double A { get; set; }

    public Hyperbola(double a)
    {
        A = a;
    }

    public double Calculate(double x)
    {
        if (x == 0) throw new DivideByZeroException("x не може бути 0 для гіперболи.");
        return A / x;
    }

    public string GetInfo(double x)
    {
        try
        {
            return $"[Hyperbola] y = {A}/x; x = {x}; y = {Calculate(x)}";
        }
        catch (DivideByZeroException ex)
        {
            return $"[Hyperbola] x = {x}; Помилка: {ex.Message}";
        }
    }

    public object Clone() => new Hyperbola(A);

    public int CompareTo(Function other)
    {
        try
        {
            return Calculate(1).CompareTo(other.Calculate(1));
        }
        catch
        {
            return 1;
        }
    }
}

// 3. Клас для музичних дисків
public record MusicDisk(string Name, string Author, double Duration, decimal Price);

public class InvalidDurationException : Exception
{
    public InvalidDurationException(string message) : base(message) { }
}

public class InvalidInsertIndexException : Exception
{
    public InvalidInsertIndexException(string message) : base(message) { }
}

// 4. Колекція музичних дисків
public class MusicCollection : IEnumerable<MusicDisk>
{
    private List<MusicDisk> disks = new();

    public void Add(MusicDisk disk) => disks.Add(disk);

    public void RemoveByDuration(double duration)
    {
        int index = disks.FindIndex(d => d.Duration == duration);
        if (index != -1)
            disks.RemoveAt(index);
    }

    public void InsertAfter(int index, MusicDisk[] newDisks)
    {
        disks.InsertRange(index + 1, newDisks);
    }

    public IEnumerator<MusicDisk> GetEnumerator() => disks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

class Program
{
    static void Main()
    {
        // Тест 1: Двигуни
        Console.WriteLine("=== Двигуни ===");
        Engine baseEngine = new Engine("Basic-100", 120);
        CombustionEngine petrolEngine = new CombustionEngine("Petrol-2000", 200, "Petrol");
        DieselEngine dieselEngine = new DieselEngine("DieselPro", 180, 5.6);
        JetEngine jetEngine = new JetEngine("JetX", 5000, 150);

        baseEngine.Show();
        petrolEngine.Show();
        dieselEngine.Show();
        jetEngine.Show();

        // Тест 2: Математичні функції
        Console.WriteLine("\n=== Математичні функції ===");
        Function[] functions = new Function[]
        {
            new Line(2, 3),
            new Kub(1, -4, 4),
            new Hyperbola(5)
        };

        double x = 2;
        foreach (var func in functions)
        {
            Console.WriteLine(func.GetInfo(x));
        }

        Array.Sort(functions);
        foreach (var func in functions)
        {
            Console.WriteLine(func.GetInfo(1));
        }

        // Тест 3: Музичні диски
        try
        {
            List<MusicDisk> disks = new()
            {
                new MusicDisk("Album A", "Artist X", 45.0, 200),
                new MusicDisk("Album B", "Artist Y", 38.5, 150),
                new MusicDisk("Album C", "Artist Z", 50.0, 220)
            };

            Console.WriteLine("\n=== Музичні диски ===");
            Print(disks);

            Console.Write("\nВведіть тривалість для видалення: ");
            if (!double.TryParse(Console.ReadLine(), out double durationToDelete))
                throw new FormatException("Невірний формат тривалості!");

            int indexToRemove = disks.FindIndex(d => d.Duration == durationToDelete);
            if (indexToRemove == -1)
                throw new InvalidDurationException("Диск з вказаною тривалості не знайдено.");

            disks.RemoveAt(indexToRemove);

            Console.Write("\nВведіть індекс (починаючи з 0), після якого додати два нові елементи: ");
            if (!int.TryParse(Console.ReadLine(), out int insertIndex))
                throw new FormatException("Невірний формат індексу!");

            if (insertIndex < -1 || insertIndex >= disks.Count)
                throw new InvalidInsertIndexException("Недопустимий індекс для вставки!");

            disks.InsertRange(insertIndex + 1, new[] {
                new MusicDisk("New Album 1", "New Artist 1", 42.0, 180),
                new MusicDisk("New Album 2", "New Artist 2", 37.5, 160)
            });

            Print(disks);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static void Print(List<MusicDisk> disks)
    {
        foreach (var d in disks)
        {
            Console.WriteLine($"{d.Name} | {d.Author} | {d.Duration} min | {d.Price:C}");
        }
    }
}
