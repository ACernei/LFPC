using System.Net.Http.Headers;
using System.Text;
using ConsoleTables;
using Lab5.Extensions;

namespace Lab5;

public static class SpFirstLast
{
    public static Dictionary<Symbol, HashSet<Symbol>> GetFirstMatrix(this Grammar grammar)
    {
        return grammar.Productions.ToDictionary(
            production => production.Key,
            production => GetFirstLast(grammar, production.Key, state => state.First()));
    }

    public static Dictionary<Symbol, HashSet<Symbol>> GetLastMatrix(this Grammar grammar)
    {
        return grammar.Productions.ToDictionary(
            production => production.Key,
            production => GetFirstLast(grammar, production.Key, state => state.Last()));
    }

    private static HashSet<Symbol> GetFirstLast(Grammar grammar, Symbol key, Func<State, Symbol> firstLastSelector,
        HashSet<Symbol>? firstLastSymbols = null)
    {
        firstLastSymbols ??= new HashSet<Symbol>();
        foreach (var state in grammar.Productions[key])
        {
            var selectedSymbol = firstLastSelector(state);
            var alreadyProcessed = firstLastSymbols.Contains(selectedSymbol);
            firstLastSymbols.Add(selectedSymbol);
            if (selectedSymbol.IsTerminal() || selectedSymbol.Equals(key)
                                            || (selectedSymbol.IsNonTerminal() && alreadyProcessed))
                continue;

            // Add first symbols for this NonTerminal
            firstLastSymbols.UnionWith(GetFirstLast(grammar, selectedSymbol, firstLastSelector, firstLastSymbols));
        }

        return firstLastSymbols;
    }

    public static string ToFirstLastString(this Grammar grammar)
    {
        var first = grammar.GetFirstMatrix();
        var last = grammar.GetLastMatrix();
        var table = new ConsoleTable(new ConsoleTableOptions
        {
            EnableCount = false,
            Columns = new[] {string.Empty, "First", "Last"}
        });
        
        grammar.Productions.Select(p => p.Key).OrderBy(k => k).ToList().ForEach(key =>
            table.AddRow(key, first[key].ToString(", "), last[key].ToString(", ")));
        return table.ToMinimalString();
    }
}