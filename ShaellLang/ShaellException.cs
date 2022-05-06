using System;
using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace ShaellLang;

public class ShaellException : StackTracedException
{
    public IValue ExceptionValue { get; }
    public int ErrorCode { get; }

    public ShaellException(IValue exceptionValue, int errorCode = 1) : base("Shaell error was thrown")
    {
        ExceptionValue = exceptionValue;
        ErrorCode = errorCode;
    }

    public override string ToString()
    {
        return $"Uncaught Shaell Exception:\n{ExceptionValue}\n{base.ToString()}";
    }
}