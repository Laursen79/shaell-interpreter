using System;

namespace ShaellLang;

public class SyntaxErrorException : Exception
{
    public SyntaxErrorException(string message) : base(message)
    {
    }
}