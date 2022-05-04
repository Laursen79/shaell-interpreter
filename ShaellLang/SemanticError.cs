using System;
using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace ShaellLang;

public class SemanticError : StackTracedException
{
    private IToken _offendingTokenStart;
    private IToken _offendingTokenEnd;

    public SemanticError(string message, IToken offendingTokenStart) 
        : base(message)
    {
        _offendingTokenStart = offendingTokenStart;

    }
    
    public SemanticError(string message, IToken offendingTokenStart, IToken offendingTokenEnd)
        : base(message)
    {
        _offendingTokenStart = offendingTokenStart;
        _offendingTokenEnd = offendingTokenEnd;
    }
    
    public override string ToString()
    {
        string rv = "";
        if (_offendingTokenEnd == null)
        {
            rv = $"Semantic error at token {_offendingTokenStart.Line}:{_offendingTokenStart.Column}: {Message}\nstack trace:";
        }
        else
        {
            rv = $"Semantic error at token spanning from {_offendingTokenStart.Line}:{_offendingTokenStart.Column} - {_offendingTokenEnd.Line}:{_offendingTokenEnd.Column} {Message}\nstack trace:";
        }
        rv += $"\n{base.ToString()}";
        return rv;
    }
}