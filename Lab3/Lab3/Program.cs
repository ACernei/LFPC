
using Lab3;

if (args.Length != 1)
{
    Console.WriteLine("Please specify one input file.");
    return;
}

var fileName = args[0];
var text = File.ReadAllText(fileName);

var lexer = new Lexer(text);
var tokens = lexer.Tokenizer();
foreach (var token in tokens)
{
    Console.WriteLine(token);
}
