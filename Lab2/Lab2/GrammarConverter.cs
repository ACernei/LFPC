using Lab2.Extensions;

namespace Lab2;

public static class GrammarConverter
{
    public static Grammar ToDfa(Grammar nfaGrammar)
    {
        var dfaGrammar = new Grammar
        {
            Start = nfaGrammar.Start,
            Edges = nfaGrammar.Edges
        };
        
        var newNodes = new List<List<string>> {new() {nfaGrammar.Start}};
        
        var stepNumber = 0;

        while (newNodes.Count != 0)
        {
            var unprocessedNodes = new List<List<string>>();
            newNodes.ForEach(dfaNode =>
            {
                // copy rows from NFA table for this new node
                var nfaTransitions = dfaNode.Select(nfaNode => nfaGrammar.Transitions[nfaNode]);
                
                var dfaNodeName = dfaNode.ConcatSorted();
                
                // create empty row in DFA for this new node
                dfaGrammar.Transitions[dfaNodeName] = new Dictionary<string, List<string>>();
                
                // sum the selected NFA rows to form the new DFA row
                dfaGrammar.Edges.ForEach(edge =>
                {
                    // don't process this edge if noone has it
                    if (nfaTransitions.All(t => !t.ContainsKey(edge)))
                        return;
                    
                    // for current edge sum NFA nodes
                    var unprocessedNode = nfaTransitions.Where(t => t.ContainsKey(edge))
                        .SelectMany(t => t[edge]).Distinct().ToList();
                    // add this calculated node to unprocessed list
                    unprocessedNodes.Add(unprocessedNode);
                    // add this calculated node to DFA
                    dfaGrammar.Transitions[dfaNodeName][edge] = unprocessedNode;
                });
                
            });
            // nodes not found in DFA are considered as new nodes that will be processed in the next iteration
            newNodes = unprocessedNodes.Where(dfaNode =>
                    !dfaGrammar.Transitions.ContainsKey(dfaNode.ConcatSorted())).ToList();
            
            // step-by-step printing
            Console.WriteLine($"Step {stepNumber}:");
            stepNumber++;
            Console.WriteLine(dfaGrammar);
        }

        dfaGrammar.Nodes = dfaGrammar.Transitions.Keys.ToList();
        dfaGrammar.Terminals = dfaGrammar.Nodes.Where(node => nfaGrammar.Terminals.Any(node.Contains)).ToList();
        return dfaGrammar;
    }
}
