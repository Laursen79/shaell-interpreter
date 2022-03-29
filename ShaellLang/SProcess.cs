using System;
using System.Collections.Generic;

namespace ShaellLang;

public class SProcess : IFunction
{
    public bool ToBool()
    {
        throw new System.NotImplementedException();
    }

    public Number ToNumber()
    {
        throw new System.NotImplementedException();
    }

    public IFunction ToFunction()
    {
        throw new System.NotImplementedException();
    }

    public SString ToSString()
    {
        throw new System.NotImplementedException();
    }

    public ITable ToTable()
    {
        throw new System.NotImplementedException();
    }

    public IValue Call(ICollection<IValue> args)
    {
        var jo = JobObject.Factory.ProcessCall(() =>
        {
            int x = 0;
            for (int i = 0; i < 1000; i++)
            {
                x += i + 3;
            }

            return x;
        });
        return jo;
    }

    public uint ArgumentCount { get; }
}