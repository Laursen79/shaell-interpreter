using System;

namespace ShaellLang;

public class UnsupportedTypeException : Exception
{
    public Type Type { get; }
    public UnsupportedTypeException(Type type)
    {
        Type = type;
    }
    public UnsupportedTypeException(Type type, string message) : base(message)
    {
        Type = type;
    }
}