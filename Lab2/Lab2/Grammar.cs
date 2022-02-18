using System.Text;
using Lab2.Extensions;

namespace Lab2;

public class Grammar
{
    public string Start { get; set; } = string.Empty;
    public List<string> Nodes { get; set; } = new();
    public List<string> Edges { get; set; } = new();
    public List<string> Terminals { get; set; } = new();
    public Dictionary<string, Dictionary<string, List<string>>> Transitions { get; set; } = new();

    // list printing format
    public override string ToString()
    {
        var sb = new StringBuilder();
        Transitions.ToList().ForEach(t =>
        { 
            sb.Append(t.Key).Append(" : { ");
            t.Value.ToList().ForEach(edge =>
            {
                sb.Append(edge.Key).Append(" : ").Append(edge.Value.ConcatSorted()).Append(", ");
            });
            sb.AppendLine("},");
        });
        return sb.ToString();
    }
}
