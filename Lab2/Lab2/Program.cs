using Lab2;

if (args.Length != 1)
{
    Console.WriteLine("Please specify one input file.");
    return;
}

var fileName = args[0];
var rows = File.ReadAllLines(fileName).ToList();
// rows.ForEach(Console.WriteLine);

var myGrammar = GrammarParser.Parse(rows);
var dfa = GrammarConverter.ToDfa(myGrammar);

Console.WriteLine($"NFA to DFA final result:\n{dfa}");

