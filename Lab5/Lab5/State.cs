using System.Collections;

namespace Lab5;

public class State : IEquatable<State>, IEnumerable<Symbol>
{
    private readonly List<Symbol> symbols;

    public State(string state)
    {
        symbols = state.Select(s => new Symbol(s)).ToList();
    }

    public Symbol First() => symbols.First();

    public Symbol Last() => symbols.Last();

    public bool IsEmpty() => symbols.Count == 1 && symbols[0].IsEpsilon();

    public HashSet<Symbol> GetNonTerminals() => symbols.Where(s => s.IsNonTerminal()).ToHashSet();

    public HashSet<Symbol> GetTerminals() => symbols.Where(s => s.IsTerminal()).ToHashSet();

    public override string ToString() => string.Join("", symbols);

    public bool Equals(State? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ToString() == other.ToString();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((State) obj);
    }

    public override int GetHashCode() => ToString().GetHashCode();

    public IEnumerator<Symbol> GetEnumerator() => symbols.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}