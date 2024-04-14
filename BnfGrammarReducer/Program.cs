
namespace BnfGrammarReducer;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: dotnet BnfGrammarReducer <input_file> <output_file>");
            return;
        }

        var inputFilePath = args[0];
        var outputFilePath = args[1];

        try
        {
            var grammars = Parser.ReadFromGrammar(inputFilePath);
            if (grammars.Count == 0)
            {
                Console.WriteLine("No grammars parsed from the file.");
                return;
            }

            string startSymbol = grammars.First().Set.Item1;
            var nT = Grammar.FilterDerivableGrammars(grammars);
            var vD = Grammar.FilterReachableGrammars(nT, startSymbol);

            Parser.WriteFormattedGrammar(outputFilePath, vD);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

