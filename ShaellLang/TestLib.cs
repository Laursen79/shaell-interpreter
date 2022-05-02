using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public static class TestLib
{
    public static bool testFailed { get; private set; }
    
    public static IValue CreateLib()
    {
        testFailed = false;
        var userTable = new UserTable();
        userTable
            .GetValue(new SString("assert"))
            .Set(new NativeFunc(AssertFunc, 2));
        userTable
            .GetValue(new SString("assertType"))
            .Set(new NativeFunc(AssertTypeFunc, 2));
        userTable
            .GetValue(new SString("describe"))
            .Set(new NativeFunc(DescribeFunc, 2));
        userTable
            .GetValue(new SString("tableEqual"))
            .Set(new NativeFunc(TableEqualFunc, 2));
        userTable
            .GetValue(new SString("arrayEquivalent"))
            .Set(new NativeFunc(ArrayEquivalentFunc, 2));
        return userTable;
    }

    private static IValue TableEqualFunc(IEnumerable<IValue> args)
    {
        var argArr = args.ToArray();
        if (argArr.Length < 3)
        {
            throw new Exception("assert: too few arguments");
        }
        
        if (argArr[0].ToBool() == false)
        {
            throw new Exception("assert: " + argArr[2].ToSString().Val);
        }

        var a = argArr[0].ToTable() as BaseTable;
        var b = argArr[1].ToTable() as BaseTable;
        var a_keys = a.GetKeys().ToArray();
        var b_keys = b.GetKeys().ToArray();
        if (a_keys.Length != b_keys.Length)
        {
            Console.WriteLine($"Expected: {a} got {b}");
            Console.WriteLine($"Failed on length check");
            
            throw new Exception("assert: " + argArr[2].ToSString().Val);
        }

        foreach (var key in a_keys)
        {
            if (!b.GetValue(key).Unpack().IsEqual( a.GetValue(key).Unpack()))
            {
                Console.WriteLine($"Expected: {a} got {b}");
                Console.WriteLine($"Failed on key {key.Serialize()} with value left: {a.GetValue(key)}, right: {b.GetValue(key)}");
                throw new Exception("assert: " + argArr[2].ToSString().Val);
            }
        }
        return new SNull();
    }

    private static IValue AssertFunc(IEnumerable<IValue> args)
    {
        var argArr = args.ToArray();
        if (argArr.Length < 2)
        {
            throw new Exception("assert: too few arguments");
        }
        
        if (argArr[0].ToBool() == false)
        {
            throw new Exception("assert: " + argArr[1].ToSString().Val);
        }

        return new SNull();
    }
    
    private static IValue AssertTypeFunc(IEnumerable<IValue> args)
    {
        var argArr = args.ToArray();
        if (argArr.Length < 2)
        {
            throw new Exception("assert: too few arguments");
        }
        
        if (argArr[0].GetTypeName() != argArr[1].ToSString().Val)
        {
            throw new Exception($"assert: expected type {argArr[1].ToSString().Val} but got {argArr[0].GetTypeName()}");
        }

        return new SNull();
    }

    private static IValue DescribeFunc(IEnumerable<IValue> args)
    {
        var argArr = args.ToArray();
        if (argArr.Length < 2)
        {
            throw new Exception("describe: too few arguments");
        }

        var name = argArr[0].ToSString().Val;
        var func = argArr[1].ToFunction();
        
        try
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Testing \"{name}\"");
            Console.ResetColor();

            func.Call(Enumerable.Empty<IValue>());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Test \"{name}\" succeeded");
            Console.ResetColor();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Test \"{name}\" failed");
            Console.ResetColor();
            Console.WriteLine(e);
            testFailed = true;
        }
        
        return new SNull();
    }

    private static IValue ArrayEquivalentFunc(IEnumerable<IValue> args)
    {
        var argArr = args.ToArray();
        if (argArr.Length < 3)
        {
            throw new Exception("array equivalent: too few arguments");
        }

        var a = argArr[0].ToTable() as BaseTable;
        var b = argArr[1].ToTable() as BaseTable;
        var aKeys = a.GetKeys().ToArray();
        var bKeys = b.GetKeys().ToArray();
        if (aKeys.Length != bKeys.Length)
        {
            Console.WriteLine($"Expected: {a} got {b}");
            Console.WriteLine($"Failed on length check");
            throw new Exception("assert: " + argArr[2].ToSString().Val);
        }

        for (var aIndex = 0; aIndex < a.ArrayLength; aIndex++)
        {
            var found = false;
            var val = a.GetValue(new Number(aIndex));
            for (var bIndex = 0; bIndex < b.ArrayLength; bIndex++)
            {
                if (val.IsEqual(b.GetValue(new Number(bIndex)).Unpack()))
                {
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine($"Expected: {a} got {b}");
                Console.WriteLine($"Failed on key {aIndex} with value {val.Serialize()}");
                throw new Exception("assert: " + argArr[2].ToSString().Val);
            }
        }
        
        return new SNull();
    }
}