namespace Lab4;

public class RemoveUnit
{
    private Dictionary<string, List<string>> unitCollection = new();

    private bool IsUnit(string state) => state.Length == 1 && char.IsUpper(state[0]);

    public bool HasUnit(Dictionary<string, List<string>> productions)
    {
        //checks if any state of 1 uppercase letter
        return productions.Any(production =>
            production.Value.Any(IsUnit));
    }

    public void AddUnits(Dictionary<string, List<string>> productions)
    {
        unitCollection = productions.Where(p => p.Value.Any(IsUnit))
            .ToDictionary(
                p => p.Key,
                p => p.Value.Where(IsUnit).ToList());
    }

    public void AddStates(Dictionary<string, List<string>> productions)
    {
        unitCollection.ToList().ForEach(unit =>
            unit.Value.ForEach(unitState =>
            {
                //remove unit state from productions
                //remove(S -> B) 
                productions[unit.Key].Remove(unitState);
                // add to the unit all the needed states
                productions[unitState]
                    .Where(state => !productions[unit.Key].Contains(state))
                    .ToList()
                    .ForEach(state => productions[unit.Key].Add(state));
            }));
    }
}