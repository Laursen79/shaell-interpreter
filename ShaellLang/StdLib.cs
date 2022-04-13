using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ShaellLang;

public class StdLib
{
    public static IValue PrintFunc(IEnumerable<IValue> args)
    {
        foreach (var value in args)
            Console.Write(value);
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

        if (Path.IsPathRooted(argArr[0].ToString()))
        {
            Directory.SetCurrentDirectory(argArr[0].ToString());
        }
        else
        {
            Directory.SetCurrentDirectory(Path.GetFullPath(Path.Join(Environment.CurrentDirectory,
                argArr[0].ToString())));
        }

        return new SNull();
    }

    public static IValue ExitFunc(IEnumerable<IValue> args)
    {
        var argArr = args.ToArray();

        Environment.Exit(Int32.Parse(argArr[0].ToString()));
        return new SNull();
    }
}