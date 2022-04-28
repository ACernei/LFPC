using System.Text;

namespace Lab5;

public class Grammar
{
    public readonly Dictionary<Symbol, HashSet<State>> Productions;

    public Grammar(Dictionary<Symbol, HashSet<State>> productions)
    {
        Productions = productions;
    }

    public HashSet<Symbol> GetNonTerminals() => Productions
        .SelectMany(p =>
            p.Value.SelectMany(s => s.GetNonTerminals()))
        .Union(Productions.Keys)
        .ToHashSet();

    public HashSet<Symbol> GetTerminals() => Productions
        .SelectMany(p =>
            p.Value.SelectMany(s => s.GetTerminals()))
        .ToHashSet();

    public override string ToString()
    {
        var grammarSb = new StringBuilder();
        Productions.ToList().ForEach(production =>
            grammarSb.AppendLine($"{production.Key} -> {string.Join(" | ", production.Value)}"));
        return grammarSb.ToString();
    }
}