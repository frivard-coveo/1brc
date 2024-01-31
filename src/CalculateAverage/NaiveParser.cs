﻿namespace CalculateAverage;

internal class NaiveParser
{
    private readonly string fileToRead;

    public NaiveParser(string fileToRead)
    {
        this.fileToRead = fileToRead;
    }

    public async ValueTask ParseAsync()
    {
        var data  = new Dictionary<string, TemperatureData>(StringComparer.Ordinal);

        await foreach (var line in File.ReadLinesAsync(fileToRead))
        {
            var parts = line.Split(';');
            if (!data.ContainsKey(parts[0]))
            {
                data.Add(parts[0], new TemperatureData());
            }
            data[parts[0]].AddSample(double.Parse(parts[1]));
        }

        foreach(var entry in data.OrderBy(e => e.Key))
        {
            Console.WriteLine($"{entry.Key}={entry.Value}");
        }
    }
}

public record TemperatureData
{
    public double Min { get; set;}
    public double Max { get; set; }
    public double Sum { get; set; }
    public int Count { get; set; }

    public double Average => Sum / Count;

    public TemperatureData()
    {
        Min = 100.0;
        Max = -100.0;
        Sum = 0.0;
        Count = 0;
    }

    public void AddSample(double value)
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

    public override string ToString() => $"{Min}/{Average:F1}/{Max}";
}