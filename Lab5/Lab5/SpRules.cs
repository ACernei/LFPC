using System.Text;
using Lab5.Extensions;

namespace Lab5;

public static class SpRules
{
    public static HashSet<Rule> GetRules(this Grammar grammar)
    {
        return GetRules(grammar, GetFirstRule)
            .Union(GetRules(grammar, GetSecondRule))
            .Union(GetRules(grammar, GetThirdRule))
            .ToHashSet();
    }

    private static HashSet<Rule> GetRules(Grammar grammar, Func<Grammar, State, HashSet<Rule>> ruleSelector)
    {
        return grammar.Productions.SelectMany(production =>
                production.Value
                    .Where(state => state.Count() > 1)
                    .ToList()
                    .SelectMany(state => ruleSelector(grammar, state)))
            .ToHashSet();
    }

    private static HashSet<Rule> GetFirstRule(Grammar grammar, State state)
    {
        return state
            .Zip(state.Skip(1), (s1, s2) => new Rule(s1, new Symbol("="), s2))
            .ToHashSet();
    }

    private static HashSet<Rule> GetSecondRule(Grammar grammar, State state)
    {
        var firstMatrix = grammar.GetFirstMatrix();
        return state
            .Zip(state.Skip(1), (s1, s2) => new Rule(s1, new Symbol("<"), s2))
            .Where(rule => rule.Right.IsNonTerminal())
            .SelectMany(rule => firstMatrix[rule.Right].Select(right => new Rule(rule.Left, rule.Op, right)))
            .ToHashSet();
    }

    private static HashSet<Rule> GetThirdRule(Grammar grammar, State state)
    {
        var lastMatrix = grammar.GetLastMatrix();
        var firstMatrix = grammar.GetFirstMatrix();
        return state
            .Zip(state.Skip(1), (s1, s2) => new Rule(s1, new Symbol(">"), s2))
            .Where(rule => rule.Left.IsNonTerminal())
            .SelectMany(rule =>
            {
                var rightCandidates = new List<Symbol> {rule.Right};
                if (rule.Right.IsNonTerminal())
                    rightCandidates = firstMatrix[rule.Right].Where(s => s.IsTerminal()).ToList();
                return lastMatrix[rule.Left].SelectMany(left =>
                    rightCandidates.Select(right => new Rule(left, rule.Op, right)));
            })
            .ToHashSet();
    }

    public static string ToRulesString(this Grammar grammar)
    {
        var rulesSb = new StringBuilder();
        rulesSb.Append("First Rule: ").AppendLine(GetRules(grammar, GetFirstRule).ToString(", "))
            .Append("Second Rule: ").AppendLine(GetRules(grammar, GetSecondRule).ToString(", "))
            .Append("Third Rule: ").AppendLine(GetRules(grammar, GetThirdRule).ToString(", "));
        return rulesSb.ToString();
    }
}