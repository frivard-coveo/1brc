using System.Diagnostics;

namespace CalculateAverage;

class Program
{
    static int Main(string[] args)
    {
        if(args.Length > 0)
        {
            Stopwatch sw = new();
            sw.Start();
            string fileToRead = args[0];
            var parser = new ByteLinesParser(fileToRead);
            //var parser = new NaiveParser(fileToRead);
            parser.Parse();

            Console.WriteLine($"Computed values in {sw.ElapsedMilliseconds} ms.");
        }
        return 0;
    }
}
