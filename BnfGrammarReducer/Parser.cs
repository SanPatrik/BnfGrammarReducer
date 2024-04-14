namespace BnfGrammarReducer;

public class Parser
{
    private const string EmptyLanguage = "PRAZDNY JAZYK";

    public static List<Grammar> ReadFromGrammar(string filePath)
    {
        var grammars = new List<Grammar>();
        string fileContent = File.ReadAllText(filePath);
        string[] lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(new[] { "::=" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) throw new FormatException("Invalid production rule format.");

            string left = parts[0].Trim();
            string[] alternatives = parts[1].Trim().Split('|');

            grammars.AddRange(alternatives.Select(alt => new Grammar(left, alt.Trim())));
        }

        return grammars;
    }
    
    public static void WriteFormattedGrammar(string filePath, List<Grammar> grammars)
    {
        if (grammars.Count == 0)
        {
            File.WriteAllText(filePath, EmptyLanguage);
            return;
        }

        var grammarGroups = grammars
            .GroupBy(g => g.Set.Item1)
            .ToDictionary(group => group.Key, group => group.Select(g => g.Set.Item2).ToList());

        using (var writer = new StreamWriter(filePath))
        {
            foreach (var grammar in grammars)
            {
                if (grammarGroups.TryGetValue(grammar.Set.Item1, out List<string> productions))
                {
                    if (productions != null)
                    {
                        writer.WriteLine($"{grammar.Set.Item1} ::= {string.Join(" | ", productions.Distinct())}");
                        grammarGroups.Remove(grammar.Set.Item1);
                    }
                }
            }
        }
    }
}