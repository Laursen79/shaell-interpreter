using Antlr4.Runtime;

namespace ShaellLang;

public class ShaellLexerErrorListener : IAntlrErrorListener<int>
{
    public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg,
        RecognitionException e)
    {
        throw new SyntaxErrorException($"Syntax error at line {line}:{charPositionInLine}");
    }
}