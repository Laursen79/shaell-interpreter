using System;

namespace ShaellLang;

public class ShaellException : Exception
{
    public IValue ExceptionValue { get; }
    
    public ShaellException(IValue exceptionValue)
    {
        ExceptionValue = exceptionValue;
    }

    public override string ToString()
    {
        return $"Uncaught Shaell Exception:\n{ExceptionValue}";
    }
}