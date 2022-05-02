using System;

namespace ShaellLang;

public abstract class BaseValue : IValue
{

    public virtual bool ToBool() => throw new Exception($"Cannot convert {GetType()} to bool");
    public virtual Number ToNumber() => throw new Exception($"Cannot convert {GetType()} to number");
    public virtual IFunction ToFunction() => throw new Exception($"Cannot convert {GetType()} to function");
    public virtual SString ToSString() => throw new Exception($"Cannot convert {GetType()} to string");
    public virtual ITable ToTable() => throw new Exception($"Cannot convert {GetType()} to table");
    public virtual SProcess ToSProcess() => throw new Exception($"Cannot convert {GetType()} to process");

    public override string ToString() => ToSString().Val;

    public abstract bool IsEqual(IValue other);

    public virtual IValue Unpack() => this;
}