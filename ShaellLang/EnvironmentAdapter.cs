using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class EnvironmentAdapter : BaseValue, ITable
{
    public EnvironmentAdapter()
        : base("table")
    {
        
    }

    public override bool IsEqual(IValue other)
    {
        return this == other;
    }

    public RefValue GetValue(IValue key)
    {
        return new EnvironmentVariableProxy(key.ToSString().Val);
    }

    public IEnumerable<IValue> GetKeys()
    {
        return Environment.GetEnvironmentVariables()
            .Keys
            .Cast<string>()
            .Select(x => new SString(x));
    }

    public override ITable ToTable() => this;

    public override bool ToBool() => true;
}