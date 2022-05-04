using System;
using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace ShaellLang;

public class StackTracedException : Exception
{
    public struct StackTraceElement
    {
        public string message;
        public ParserRuleContext context;
    }
    
    private List<StackTraceElement> _stackTrace;
    
    public IEnumerable<StackTraceElement> StackTrace => _stackTrace;
    
    public StackTracedException(string message) : base(message)
    {
        _stackTrace = new List<StackTraceElement>();
    }

    public void AddTrace(ParserRuleContext context, string message)
    {
        _stackTrace.Add(new StackTraceElement {
            message = message,
            context = context
        });
    }

    public override string ToString()
    {
        var rv = "";
        foreach (var trace in StackTrace)
        {
            int start = trace.context.start.StartIndex;
            int stop = trace.context.stop.StopIndex;
            rv +=
                $"{trace.context.start.InputStream.SourceName} [{trace.context.start.Line}:{trace.context.start.Column} - {trace.context.stop.Line}:{trace.context.stop.Column}]\n";
            var offendingText = trace.context.start.InputStream.GetText(new Interval(start, stop)).Split("\n");
            if (offendingText.Length > 1)
                rv += $"{offendingText[0]}...\n";
            else
                rv += $"{offendingText[0]}\n";

        }
        return rv;
    }
}