using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class NumberTable : NativeTable
{
    private static NumberTable instance = new NumberTable();
    public static Number Number;
    
    private NumberTable()
    {
        SetValue("sqrt", new NativeFunc(SqrtCallHandler, 0));
        SetValue("floor", new NativeFunc(FloorCallHandler, 0));
        SetValue("ceil", new NativeFunc(CeilCallHandler,0));
        SetValue("log2", new NativeFunc(Log2CallHandler,0));
        SetValue("log", new NativeFunc(LogCallHandler,0));
    }

    public static NumberTable GetInstance(Number caller)
    {
        Number = caller;
        return instance;
    }
    
    private static IValue SqrtCallHandler(IEnumerable<IValue> args)
        => new Number(Math.Sqrt(Number.ToFloating()));

    private static IValue FloorCallHandler(IEnumerable<IValue> args)
        => new Number(Math.Floor(Number.ToFloating()));

    private static IValue CeilCallHandler(IEnumerable<IValue> args)
        => new Number(Math.Ceiling(Number.ToFloating()));

    private static IValue Log2CallHandler(IEnumerable<IValue> args)
        => new Number(Math.Log2(Number.ToFloating()));

    private static IValue LogCallHandler(IEnumerable<IValue> args)
        => new Number(Math.Log(Number.ToFloating(), args.ToArray()[0].ToNumber().ToFloating()));
}