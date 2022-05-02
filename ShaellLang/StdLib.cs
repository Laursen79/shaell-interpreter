using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace ShaellLang;

public class StdLib
{
    public static IValue PrintFunc(IEnumerable<IValue> args)
    {
        foreach (var value in args)
            Console.Write(value.ToSString().Val);
        Console.WriteLine();

        return new SNull();
    }

    public static IValue DebugBreakFunc(IEnumerable<IValue> args)
    {
        Console.WriteLine("Debug break");
        return new SNull();
    }
    
    public static IValue CdFunc(IEnumerable<IValue> args)
    {
        var argArr = args.ToArray();
        if (argArr.Length < 1)
        {
            throw new ShaellException(new SString("Expected 1 argument"));
        }

        var givenPath = argArr[0].ToString();
        if (Path.IsPathRooted(givenPath))
        {
            Directory.SetCurrentDirectory(givenPath);
        }
        else
        {
            Directory.SetCurrentDirectory(Path.GetFullPath(Path.Join(Environment.CurrentDirectory,
                givenPath)));
        }

        return new SNull();
    }

    public static IValue ExitFunc(IEnumerable<IValue> args)
    {
        var argArr = args.ToArray();
        int returnCode = 0;
        if (argArr.Length < 1)
        {
            returnCode = 0;
        }
        else
        {
            var val = argArr[0].ToNumber();
            if (!val.IsInteger)
            {
                throw new ShaellException(new SString("Expected integer as first argument"));
            }

            long longVal = val.ToInteger();
            if (longVal >= int.MaxValue || longVal <= int.MinValue)
            {
                throw new ShaellException(new SString($"Expected integer in range of {int.MinValue} to {int.MaxValue} as first argument"));
            }

            returnCode = (int) longVal;
        }

        Environment.Exit(returnCode);
        return new SNull();
    }
}