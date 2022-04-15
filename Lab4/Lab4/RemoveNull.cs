namespace Lab4;

public class RemoveNull
{
    private HashSet<string> epsilonStates = new();
    private const string epsilon = "ε";

    public bool HasEpsilon(Dictionary<string, List<string>> productions)
    {
        //checks if grammar has an epsilon
        return productions
            .Any(production => production.Value.Contains(epsilon));
    }

    public void SetEpsilonStates(Dictionary<string, List<string>> productions)
    {
        //add Non Terminal states that have epsilon
        epsilonStates.UnionWith(productions
            .Where(t => t.Value.Contains(epsilon))
            .Select(t => t.Key));
    }

    public void AddNewEpsilonStatesToProductions(Dictionary<string, List<string>> productions)
    {
        // add new states that had an epsilonState
        epsilonStates.ToList().ForEach(epsilonState =>
        {
            //remove the epsilon from epsilonState
            productions[epsilonState].Remove(epsilon);

            //add new states that have epsilon (ex: epsilonState = A)
            productions.ToList().ForEach(production =>
            {
                //in case the state maps to the epsilonState (ex: S -> A, A -> ε)
                productions[production.Key] =
                    productions[production.Key].Select(s => s == epsilonState ? epsilon : s).ToList();

                production.Value.Where(s => s.Contains(epsilonState)).ToList().ForEach(state =>
                {
                    //break down a big state into smaller (ex: S -> ABsA, A -> ε)
                    var newStates = GenerateStates(state, epsilonState);
                    productions[production.Key].AddRange(newStates.Except(productions[production.Key]));
                });
            });
        });
    }

    private List<string> GenerateStates(string input, string epsilonState)
    {
        //decompose the state into smaller states, by removing one A at a time (substring)
        var states = new HashSet<string>(); //temp list that will store all decomposed states
        var queue = new Queue<string>(new[] {input});
        while (queue.Count != 0)
        {
            var state = queue.Dequeue();
            state.Select((symbol, i) => new {Char = symbol, Index = i}) // pair char with index
                .Where(symbol => symbol.Char.ToString() == epsilonState) // positions that equals A
                .Select(symbol => state.Remove(symbol.Index, 1)) // create substate by removing an A
                .ToList()
                .ForEach(subState =>
                {
                    if (subState.Length == 0) //handle if multiple A's result in a empty string
                        subState = epsilon;
                    else
                        queue.Enqueue(subState);

                    states.Add(subState);
                });
        }

        return states.ToList();
    }
}