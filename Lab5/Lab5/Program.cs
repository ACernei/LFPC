namespace Lab5;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Please specify two input files.");
            return;
        }

        var grammarFile = args[0];
        var grammarText = File.ReadAllLines(grammarFile);
        var productions = ParseProductions(grammarText);

        var grammar = new Grammar(productions);
        Console.WriteLine(grammar.ToFirstLastString());
        Console.WriteLine(grammar.ToRulesString());
        Console.WriteLine(grammar.ToSpMatrixString());

        var wordsFile = args[1];
        var words = File.ReadAllLines(wordsFile).ToList();
        words.ForEach(word =>
            Console.WriteLine(grammar.Parse(word)));
    }

    private static Dictionary<Symbol, HashSet<State>> ParseProductions(string[] rows)
    {
        //adds grammar from file to dictionary of arrays
        return rows
            .GroupBy(line => line[0])
            .ToDictionary(
                lineGroup => new Symbol(lineGroup.Key),
                lineGroup => lineGroup
                    .Select(line => new State(line.Substring(line.IndexOf('>') + 1)))
                    .ToHashSet()
            );
    }
}