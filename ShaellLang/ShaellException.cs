using System;
using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace ShaellLang;

public class ShaellException : StackTracedException
{
    public IValue ExceptionValue { get; }

    public ShaellException(IValue exceptionValue) : base("Shaell error was thrown")
    {
        ExceptionValue = exceptionValue;
    }

    public override string ToString()
    {
        return $"Uncaught Shaell Exception:\n{ExceptionValue}\n{base.ToString()}";
    }
}