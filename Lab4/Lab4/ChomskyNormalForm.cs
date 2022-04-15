namespace Lab4;

public class ChomskyNormalForm
{
    private char constant = 'H';

    public void TransformToNonTerminals(Dictionary<string, List<string>> productions)
    {
        //change transactions with terminals and non terminals
        var rules = new Dictionary<string, string>();
        productions.ToList().ForEach(production =>
        {
            for (var i = 0; i < production.Value.Count; i++)
            {
                if (production.Value[i].Length <= 1)
                    continue;
                for (var j = 0; j < production.Value[i].Length; j++)
                {
                    if (!char.IsLower(production.Value[i][j]))
                        continue;
                    //check if we have in rules that substitution
                    if (!rules.ContainsKey(production.Value[i][j].ToString()))
                    {
                        rules[production.Value[i][j].ToString()] = constant++.ToString();
                    }

                    //take substring before terminal, the changed terminal and after terminal substring
                    var subState = production.Value[i][..j] +
                                   rules[production.Value[i][j].ToString()] +
                                   production.Value[i][(j + 1)..];
                    productions[production.Key][i] = subState;
                }
            }
        });

        //add every new rule to the transition table
        rules.ToList().ForEach(rule =>
            productions[rule.Value] = new List<string> {rule.Key});
    }

    public void TransformToChomsky(Dictionary<string, List<string>> productions)
    {
        //change transactions with more than 2 symbols
        var rules = new Dictionary<string, string>();
        while (IsLong(productions))
        {
            productions.ToList().ForEach(production =>
            {
                for (var i = 0; i < production.Value.Count; i++)
                {
                    if (production.Value[i].Length <= 2) //consider states with length >= 3
                        continue;
                    //take substrings of len 2
                    //in case state is odd len, ignore last char
                    var size = production.Value[i].Length - (production.Value[i].Length % 2);
                    var subStates = Enumerable.Range(0, size / 2)
                        .Select(index => production.Value[i].Substring(index * 2, 2))
                        .ToList();

                    subStates.ForEach(subState => //now change all that pairs
                    {
                        if (!rules.ContainsKey(subState))
                        {
                            //add the new rule and replace old Variable with new
                            rules.Add(subState, constant++.ToString());
                        }

                        var changedState = production.Value[i].Replace(subState, rules[subState]);
                        productions[production.Key][i] = changedState;
                    });
                }
            });
        }

        //add the remaining rules to the productions
        rules.ToList().ForEach(rule =>
            productions.Add(rule.Value, new List<string>() {rule.Key}));
    }

    private bool IsLong(Dictionary<string, List<string>> productions)
    {
        //checks if grammar has states longer than 2
        return productions
            .Any(production =>
                production.Value.Any(state => state.Length > 2));
    }
}