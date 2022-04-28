namespace Lab5;

public class Rule : IEquatable<Rule>, IComparable<Rule>
{
    public Symbol Left { get; }
    public Symbol Op { get; }
    public Symbol Right { get; }

    public Rule(Symbol left, Symbol op, Symbol right)
    {
        Left = left;
        Op = op;
        Right = right;
    }

    public override string ToString() => Left + Op.ToString() + Right;

    public bool Equals(Rule? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Left.Equals(other.Left) && Op.Equals(other.Op) && Right.Equals(other.Right);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Rule) obj);
    }

    public override int GetHashCode() => HashCode.Combine(Left, Op, Right);

    public int CompareTo(Rule? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var leftComparison = Left.CompareTo(other.Left);
        if (leftComparison != 0) return leftComparison;
        var opComparison = Op.CompareTo(other.Op);
        if (opComparison != 0) return opComparison;
        return Right.CompareTo(other.Right);
    }
}