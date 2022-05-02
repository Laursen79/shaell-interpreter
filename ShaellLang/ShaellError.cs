using System;
using System.Diagnostics.CodeAnalysis;

namespace ShaellLang;

public struct ShaellError
{
    public string Message;
    public Exception Exception;

    public ShaellError([NotNull] string message, Exception exception)
    {
        Message = message;
        Exception = exception;
    }
}