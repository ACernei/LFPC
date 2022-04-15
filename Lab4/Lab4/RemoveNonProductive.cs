namespace Lab4;

public class RemoveNonProductive
{
    private HashSet<string> nonTerminals = new();

    public void AddNonTerminals(Dictionary<string, List<string>> productions)
    {
        nonTerminals = new(productions
            .Where(production =>
                production.Value.Any(state => state.Length == 1 && char.IsLower(state[0])))
            .Select(production => production.Key));
    }

    public void CheckProductions(Dictionary<string, List<string>> productions)
    {
        //check Productions once more, substituting known nonTerminals in unknown nonTerminals
        //if new are found, do another iteration in case other unknowns are found
        var foundNew = true;
        while (foundNew)
        {
            foundNew = false;
            productions.Where(production => // find productions where
                    !nonTerminals.Contains(production.Key) // Key is not yet in nonTerminals
                    && production.Value.Any(state => // and there is at least a state where
                        state.Where(char.IsUpper).Select(s => s.ToString()) // all nonTerminals
                            .All(nonTerminal => nonTerminals.Contains(nonTerminal)))) // already processed
                .ToList()
                .ForEach(production =>
                {
                    nonTerminals.Add(production.Key);
                    foundNew = true;
                });
        }
    }

    public void RemoveOccurrencesKey(Dictionary<string, List<string>> productions)
    {
        //remove all occurrences of that key
        productions.Where(production => !nonTerminals.Contains(production.Key))
            .ToList()
            .ForEach(production =>
            {
                productions.Remove(production.Key);
                productions.ToList()
                    .ForEach(tmpProd =>
                        productions[tmpProd.Key].RemoveAll(state => state.Contains(production.Key)));
            });
    }
}