namespace Lab4;

public class RemoveInaccessible
{
    private HashSet<string> nonTerminals = new() {"S"}; //S is accessible by default

    public void FindNonTerminals(Dictionary<string, List<string>> productions)
    {
        //find NonTerminals in the right part
        var foundNew = true;
        while (foundNew)
        {
            foundNew = false;
            //iterate every reachable state if you can reach other states
            nonTerminals.ToList().SelectMany(reachableNonTerminal =>
                    productions[reachableNonTerminal].SelectMany(state => state))
                .Where(symbol => char.IsUpper(symbol) && !nonTerminals.Contains(symbol.ToString()))
                .ToList()
                .ForEach(newNonTerminal =>
                {
                    nonTerminals.Add(newNonTerminal.ToString());
                    foundNew = true;
                });
        }
    }

    public void RemoveNonTerminals(Dictionary<string, List<string>> productions)
    {
        //remove those that are not in the right part
        productions.Where(production => !nonTerminals.Contains(production.Key))
            .ToList()
            .ForEach(production => productions.Remove(production.Key));
    }
}