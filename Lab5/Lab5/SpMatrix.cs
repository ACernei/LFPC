using System.Text;
using ConsoleTables;

namespace Lab5;

public static class SpMatrix
{
    private const string emptyCell = "-";

    public static Dictionary<Symbol, Dictionary<Symbol, Symbol>> GetSpMatrix(this Grammar grammar)
    {
        var headers = grammar
            .GetNonTerminals()
            .Union(grammar.GetTerminals())
            .Append(new Symbol("$"))
            .ToList();

        var matrix = headers.ToDictionary(
            header => header,
            header => InitMatrixRow(headers, header));

        var rules = grammar.GetRules();
        rules.ToList().ForEach(rule => matrix[rule.Left][rule.Right] = rule.Op);

        return matrix;
    }

    private static Dictionary<Symbol, Symbol> InitMatrixRow(List<Symbol> headers, Symbol symbol)
    {
        var delimiterCell = new Symbol(">");
        var nonDelimiterCell = new Symbol(emptyCell);
        if (symbol.IsDollar())
        {
            delimiterCell = new Symbol(emptyCell);
            nonDelimiterCell = new Symbol("<");
        }

        return headers.ToDictionary(
            header => header,
            header => header.IsDollar() ? delimiterCell : nonDelimiterCell);
    }

    public static string ToSpMatrixString(this Grammar grammar)
    {
        var matrix = grammar.GetSpMatrix();
        var headers = matrix.Keys.OrderBy(s => s).ToList();

        var table = new ConsoleTable(new ConsoleTableOptions
        {
            EnableCount = false,
            Columns = headers.Select(s => s.Value).Prepend(string.Empty)
        });

        headers.ForEach(header =>
            table.AddRow(
                // ReSharper disable once CoVariantArrayConversion
                matrix[header]
                    .OrderBy(s => s.Key)
                    .Select(s => s.Value)
                    .Prepend(header)
                    .ToArray()
            ));
        return table.ToMinimalString();
    }
}