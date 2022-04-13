using System;
using System.Reflection.PortableExecutable;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace ShaellLang;

internal class ShaellParserErrorListener : IAntlrErrorListener<IToken>
{
    ShaellErrorReporter errorReporter;
    public ShaellParserErrorListener(ShaellErrorReporter errorListener)
    {
        errorReporter = errorListener;
    }

    public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
        RecognitionException e)
    {
        var prevToken =((ITokenStream)recognizer.InputStream).Lt(-1).Text;
        errorReporter.ReportError(new ShaellError($"Unexpected token {offendingSymbol.Text} following {prevToken} on line {line}:{charPositionInLine}",e));

    }
}