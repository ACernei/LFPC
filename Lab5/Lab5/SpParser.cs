using System.ComponentModel;
using System.Text;
using Lab5.Extensions;

namespace Lab5;

public static class SpParser
{
    public static string Parse(this Grammar grammar, string input)
    {
        var inputSymbols = input.Select(s => new Symbol(s)).ToList();
        var matrix = grammar.GetSpMatrix();
        if (inputSymbols.Any(s => !matrix.ContainsKey(s)))
            return $"{input} is not part of grammar{Environment.NewLine}";
        var parserSb = new StringBuilder();

        inputSymbols = AddRuleSymbols(matrix, inputSymbols);
        parserSb.AppendLine(inputSymbols.ToString(""));

        while (true)
        {
            if (inputSymbols.ToString("") == "$<S>$")
            {
                parserSb.AppendLine($"{input} is part of grammar");
                return parserSb.ToString();
            }

            var stop = inputSymbols.IndexOf(new Symbol(">"));
            var start = inputSymbols.LastIndexOf(new Symbol("<"), stop);
            var prev = inputSymbols[start - 1];
            var next = inputSymbols[stop + 1];
            var between = new State(inputSymbols
                .Skip(start + 1).SkipLast(inputSymbols.Count - stop)
                .Where(s => !s.IsEqual())
                .ToString(""));

            var candidates = grammar.Productions
                .Where(production =>
                    production.Value.Contains(between)
                    && !matrix[prev][production.Key].IsMinus()
                    && !matrix[production.Key][next].IsMinus())
                .Select(production => production.Key)
                .ToList();
            if (!candidates.Any())
                return $"{input} is not part of grammar{Environment.NewLine}";

            var newSymbol = candidates.Count == 1
                ? candidates[0]
                : candidates.FirstOrDefault(cand =>
                      matrix[prev][cand].IsEqual() && matrix[cand][next].IsEqual())
                  ?? candidates.FirstOrDefault(cand => matrix[cand][next].IsEqual())
                  ?? candidates.FirstOrDefault(cand => matrix[prev][cand].IsEqual())
                  ?? candidates[0];

            inputSymbols = inputSymbols.Take(start)
                .Append(matrix[prev][newSymbol])
                .Append(newSymbol)
                .Append(matrix[newSymbol][next])
                .Concat(inputSymbols.Skip(stop + 1))
                .ToList();
            parserSb.AppendLine(inputSymbols.ToString(""));
        }
    }

    private static List<Symbol> AddRuleSymbols(Dictionary<Symbol, Dictionary<Symbol, Symbol>> matrix,
        List<Symbol> inputSymbols)
    {
        // Transform adbbdb into $<a<d<b>b<d<b>$
        // $adbbdb
        // adbbdb$
        return inputSymbols.Prepend(new Symbol("$"))
            .Zip(inputSymbols.Append(new Symbol("$")), (s1, s2) => new[] {s1, matrix[s1][s2]})
            .SelectMany(pair => pair)
            .Append(new Symbol("$"))
            .ToList();
    }
}