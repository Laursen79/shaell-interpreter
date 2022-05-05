using System.Collections.Generic;

namespace ShaellLang;

public class ArgumentsParser
{
    private static readonly ArgumentsParser Instance = new ArgumentsParser();
    private string[] _arguments;

    public static string[] Arguments
    {
        get => Instance._arguments.Clone() as string[];
        private set => Instance._arguments = value;
    }
    
    public static void Parse(string[] args)
    {
        Arguments = args;
    }
    
}