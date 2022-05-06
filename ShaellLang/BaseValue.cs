using System;

namespace ShaellLang;

public abstract class BaseValue : IValue
{
    private readonly string _typeName;

    protected BaseValue(string typeName)
    {
        _typeName = typeName;
    }
    public virtual bool ToBool() => throw new ShaellConversionException(_typeName, "bool");

    public virtual Number ToNumber() => throw new ShaellConversionException(_typeName, "number");
    public virtual IFunction ToFunction() => throw new ShaellConversionException(_typeName, "function");

    public virtual SString ToSString() => throw new ShaellConversionException(_typeName, "string");

    public virtual ITable ToTable() => throw new ShaellConversionException(_typeName, "table");

    public virtual SFile ToSFile() => throw new ShaellConversionException(_typeName, "file");
    
    public override string ToString() => ToSString().Val;

    public virtual SString Serialize() => throw new Exception($"Cannot serialize {_typeName}");

    public abstract bool IsEqual(IValue other);
    
    public string GetTypeName() => _typeName;

    public virtual IValue Unpack() => this;
}