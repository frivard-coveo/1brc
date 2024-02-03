using System.Diagnostics;

namespace CalculateAverage;

internal sealed class NaiveParser
{
    private readonly string fileToRead;

    public NaiveParser(string fileToRead)
    {
        this.fileToRead = fileToRead;
    }

    public void Parse()
    {
        var sw = new Stopwatch();
        sw.Start();
        var data  = new Dictionary<string, TemperatureData>(StringComparer.Ordinal);

        foreach (var line in File.ReadLines(fileToRead))
        {
            var parts = line.Split(';');
            bool found = data.TryGetValue(parts[0], out var tempData);
            if(!found)
            {
                tempData = new TemperatureData();
                data.Add(parts[0], tempData);
            }
            tempData.AddSample(ParseSample(parts[1]));// double.Parse(parts[1]));
        }
        Console.WriteLine($"Computed values in {sw.ElapsedMilliseconds} ms.");

        sw.Restart();
        Console.Write('{');
        foreach(var entry in data.OrderBy(e => e.Key))
        {
            Console.Write($"{entry.Key}={entry.Value}, ");
        }
        Console.WriteLine('}');
        Console.WriteLine($"Sorted values in {sw.ElapsedMilliseconds} ms.");
    }

    private int ParseSample(string sval)
    {
        int l = sval.Length;
        int i = 0;
        int mult = 1;
        int value = 0;
        if (sval[i] == '-')
        {
            mult = -1;
            ++i;
        }
        while(i < l)
        {
            if (sval[i] != '.')
            {
                value = (10 * value) + (sval[i]-'0');
            }
            ++i;
        }
        return mult*value;
    }
}

public record TemperatureData
{
    public int Min { get; set;}
    public int Max { get; set; }
    public long Sum { get; set; }
    public int Count { get; set; }

    public double Average => Sum / (Count*10.0);

    public TemperatureData()
    {
        Min = 1000;
        Max = -1000;
        Sum = 0;
        Count = 0;
    }

    public void AddSample(int value)
    {
        if (value < Min)
        {
            Min = value;
        }
        if (value > Max)
        {
            Max = value;
        }
        Sum += value;
        Count++;
    }

    public override string ToString() => $"{Min/10.0:F1}/{Average:F1}/{Max/10.0:F1}";
}
