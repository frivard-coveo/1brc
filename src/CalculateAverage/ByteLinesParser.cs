using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateAverage;

internal class ByteLinesParser
{
    private readonly string fileToRead;

    public ByteLinesParser(string fileToRead)
    {
        this.fileToRead = fileToRead;
    }

    public void Parse()
    {
        var names = new Dictionary<int, string>();
        var data = new Dictionary<int, TemperatureData>();

        using FileStream file = new FileStream(fileToRead, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
        byte[] buffer = new byte[256];
        int bytesRead;

        int lineStartPosition = 0;
        bool moreData = true;
        while(moreData) // we assume that a line will always fit within 256 bytes 
        {
            file.Position = lineStartPosition;
            bytesRead = file.Read(buffer, 0, buffer.Length);
            if(bytesRead < 5) // this is really the smallest possible line
            {
                moreData = false;
                break;
            }
            Span<byte> lineSpan = buffer.AsSpan(0, bytesRead);
            int index = lineSpan.IndexOf((byte)';');
            var identifierSpan = lineSpan.Slice(0, index);
            int hash = GetHash(identifierSpan);
            if (!names.ContainsKey(hash))
            {
                names.Add(hash, Encoding.UTF8.GetString(identifierSpan));
            }
            int eolIndex = lineSpan.Slice(index+1).IndexOf((byte)'\n');
            var sampleSpan = lineSpan.Slice(index+1, eolIndex);
            if(!data.ContainsKey(hash))
            {
                data.Add(hash, new TemperatureData());
            }
            data[hash].AddSample(SpanToTemperature(sampleSpan));
            lineStartPosition += index + eolIndex + 2;
        }

        Console.Write('{');
        foreach (var entry in names.OrderBy(e => e.Value, StringComparer.Ordinal))
        {
            var temp = data[entry.Key];
            Console.Write($"{entry.Value}={temp}, ");
        }
        Console.Write('}');

    }

    private int SpanToTemperature(Span<byte> span)
    {
        if (span[0] == (byte)'-')
        {
            return -1 * SpanToTemperature(span.Slice(1));
        }
        int result = 0;
        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == (byte)'.')
            {
                continue;
            }   
            result = result * 10 + (span[i] - (byte)'0');
        }
        return result;
    }

    private int GetHash(ReadOnlySpan<byte> span)
    {
        HashCode hash = new();
        hash.AddBytes(span);
        return hash.ToHashCode();
    }

    public record TemperatureData
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public long Sum { get; set; }
        public int Count { get; set; }

        public double Average => Sum / (10.0*Count);

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
}
