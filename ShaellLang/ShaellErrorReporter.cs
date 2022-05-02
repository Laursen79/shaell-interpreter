using System.Collections.Generic;
using Antlr4.Runtime;

namespace ShaellLang;

public class ShaellErrorReporter
{
    private List<ShaellError> errors = new List<ShaellError>();
    public void ReportError(ShaellError e)
    {
        errors.Add(e);
    }
    
    public bool HasErrors => errors.Count > 0;
    
    
    public override string ToString()
    {
        string result = "";
        foreach (ShaellError e in errors)
        {
            result += e.Message + "\n";
        }
        return result;
    }

    public void SetErrorListener(ShaellLexer lexer)
    {
        lexer.AddErrorListener(new ShaellLexerErrorListener(this));
    }
    
    public void SetErrorListener(ShaellParser parser)
    {
        parser.AddErrorListener(new ShaellParserErrorListener(this));
    }
    
}