using System;

namespace ShaellLang;

public class EnvironmentVariableProxy : RefValue
{
    private string _name;

    public EnvironmentVariableProxy(string name)
    {
        _name = name;
    }

    protected override IValue _RealValue
    {
        get
        {
            string? val = Environment.GetEnvironmentVariable(_name);
            if (val == null)
                return new SNull();

            return new SString(Environment.GetEnvironmentVariable(_name));

        }
        set => Environment.SetEnvironmentVariable(_name, value.ToSString().Val);
    }
}