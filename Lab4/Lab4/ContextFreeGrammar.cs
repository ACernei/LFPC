namespace Lab4;

public class ContextFreeGrammar
{
    private readonly Dictionary<string, List<string>> Productions;
    private readonly RemoveNull RemoveNull = new();
    private readonly RemoveUnit RemoveUnit = new();
    private readonly RemoveNonProductive RemoveNonProductive = new();
    private readonly RemoveInaccessible RemoveInaccessible = new();
    private readonly ChomskyNormalForm ChomskyNormalForm = new();

    public ContextFreeGrammar(Dictionary<string, List<string>> productions)
    {
        Productions = productions;
    }

    public void ConvertToCnf()
    {
        Console.WriteLine("Context free grammar:");
        PrintProductions();
        Console.WriteLine("Eliminate ε productions:");
        RemoveNullProductions();
        PrintProductions();
        Console.WriteLine("Eliminate any renaming:");
        RemoveUnitProductions();
        PrintProductions();
        Console.WriteLine("Eliminate inaccessible symbols:");
        RemoveInaccessibleProductions();
        PrintProductions();
        Console.WriteLine("Eliminate the non productive symbols:");
        RemoveNonProductiveProductions();
        PrintProductions();
        Chomsky();
        Console.WriteLine("Chomsky normal form:");
        PrintProductions();
    }

    private void PrintProductions()
    {
        Productions.ToList().ForEach(transaction =>
            Console.WriteLine($"{transaction.Key} -> {string.Join(" | ", transaction.Value)}"));
    }

    private void RemoveNullProductions()
    {
        //repeat the process of removing epsilons while any appear
        while (RemoveNull.HasEpsilon(Productions))
        {
            RemoveNull.SetEpsilonStates(Productions);
            RemoveNull.AddNewEpsilonStatesToProductions(Productions);
        }
    }

    private void RemoveUnitProductions()
    {
        //check if Productions still have unit 
        while (RemoveUnit.HasUnit(Productions))
        {
            RemoveUnit.AddUnits(Productions);
            RemoveUnit.AddStates(Productions);
        }
    }

    private void RemoveNonProductiveProductions()
    {
        RemoveNonProductive.AddNonTerminals(Productions);
        RemoveNonProductive.CheckProductions(Productions);
        RemoveNonProductive.RemoveOccurrencesKey(Productions);
    }

    private void RemoveInaccessibleProductions()
    {
        RemoveInaccessible.FindNonTerminals(Productions);
        RemoveInaccessible.RemoveNonTerminals(Productions);
    }

    private void Chomsky()
    {
        ChomskyNormalForm.TransformToNonTerminals(Productions);
        Console.WriteLine("Grammar with many non terminals: ");
        PrintProductions();
        ChomskyNormalForm.TransformToChomsky(Productions);
    }
}