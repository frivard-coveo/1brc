using System.Diagnostics;

namespace CalculateAverage;

class Program
{
    static async Task Main(string[] args)
    {
        if(args.Length > 0)
        {
            Stopwatch sw = new();
            sw.Start();
            string fileToRead = args[0];
            var parser = new NaiveParser(fileToRead);
            await parser.ParseAsync();

            Console.WriteLine($"Computed values in {sw.ElapsedMilliseconds} ms.");
        }
    }
}
