namespace Lab5;

public class Symbol : IEquatable<Symbol>, IComparable<Symbol>
{
    public string Value { get; }

    public Symbol(char symbol)
    {
        Value = symbol.ToString();
    }

    public Symbol(string symbol)
    {
        Value = symbol[0].ToString();
    }

    public bool IsTerminal() => char.IsLower(Value[0]);

    public bool IsNonTerminal() => char.IsUpper(Value[0]);

    public bool IsEpsilon() => Value == "ε";

    public bool IsDollar() => Value == "$";

    public bool IsMinus() => Value == "-";

    public bool IsEqual() => Value == "=";

    public bool IsGreater() => Value == ">";

    public bool IsLower() => Value == "<";

    public bool IsStart() => Value == "S";

    public override string ToString() => Value;

    public bool Equals(Symbol? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Symbol) obj);
    }

    public override int GetHashCode() => Value.GetHashCode();
    
    private string GetCompareValue() => IsStart() ? "@"
        : IsDollar() ? "{" : Value;

    public int CompareTo(Symbol? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(GetCompareValue(), other.GetCompareValue(), StringComparison.Ordinal);
    }
}