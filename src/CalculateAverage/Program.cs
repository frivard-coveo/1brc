namespace CalculateAverage;

class Program
{
    static async Task Main(string[] args)
    {
        if(args.Length > 0)
        {
            string fileToRead = args[0];
            var parser = new NaiveParser(fileToRead);
            await parser.ParseAsync();
        }
    }
}
