using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace ShaellLang;

public class ShaellLexerErrorListener : IAntlrErrorListener<int>
{
    public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg,
        RecognitionException e)
    { ;
        switch (e)
        {
            case LexerNoViableAltException noViableAlt:
                var offendingChar = Utils.EscapeWhitespace(((ICharStream)noViableAlt.InputStream).GetText(Interval.Of(noViableAlt.StartIndex, noViableAlt.StartIndex)), false);
                throw new SyntaxErrorException($"Syntax error at line {line}:{charPositionInLine}. Unexpected character: \"{offendingChar}\"");
        }
        
    }
}