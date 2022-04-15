namespace Lab4;

public class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Please specify one input file.");
            return;
        }

        var fileName = args[0];
        var text = File.ReadAllLines(fileName);

        var productions = Initialize(text);
        var cfg = new ContextFreeGrammar(productions);
        cfg.ConvertToCnf();
    }

    static Dictionary<string, List<string>> Initialize(string[] rows)
    {
        //adds grammar from file to dictionary of arrays
        return rows
            .GroupBy(line => line[0].ToString())
            .ToDictionary(
                lineGroup => lineGroup.Key,
                lineGroup => lineGroup
                    .Select(line => line.Substring(line.IndexOf('>') + 1)).ToList()
            );
    }
}