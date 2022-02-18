namespace Lab2;

public static class GrammarParser
{
    public static Grammar Parse(List<string> rows)
    {
        return new Grammar
        {
            Start = ParseStart(rows[0]),
            Nodes = ParseNodes(rows[1]),
            Edges = ParseEdges(rows[2]),
            Terminals = ParseTerminals(rows[3]),
            Transitions = ParseTransitions(rows.Skip(4).ToList())
        };
    }

    // get starting node
    public static string ParseStart(string row)
    {
        var separators = new char[] {' ', ','};
        var parts = row.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        return parts[5];
    }

    // get list of nodes
    public static List<string> ParseNodes(string row)
    {
        var separators = new char[] {'Q', ' ', '=', '{', ',', '}'};
        var parts = row.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        return parts.ToList();
    }

    // get list of edges
    public static List<string> ParseEdges(string row)
    {
        var separators = new char[] {'E', ' ', '=', '{', ',', '}'};
        var parts = row.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        return parts.ToList();
    }
    
    // get list of ending nodes
    public static List<string> ParseTerminals(string row)
    {
        var separators = new char[] {'F', ' ', '=', '{', ',', '}'};
        var parts = row.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        return parts.ToList();
    }
    
    // get dictionary of transitions  
    public static Dictionary<string, Dictionary<string, List<string>>> ParseTransitions(List<string> rows)
    {
        var transitions = new Dictionary<string, Dictionary<string, List<string>>>();
        rows.ForEach(row =>
        {
            var separators = new char[] {'d', '(', ',', ' ', ')', '='};
            var parts = row.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (transitions.ContainsKey(parts[0]))
            {
                var transition = transitions[parts[0]];
                if (transition.ContainsKey(parts[1]))
                {
                    transition[parts[1]].Add(parts[2]);
                }
                else
                {
                    transition[parts[1]] = new List<string> {parts[2]};
                }
            }
            else
            {
                transitions[parts[0]] = new Dictionary<string, List<string>>
                {
                    {parts[1], new List<string> {parts[2]}}
                };
            }
        });
        return transitions;
    }
}
