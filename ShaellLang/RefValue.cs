namespace ShaellLang;

public class RefValue : IValue
{
    protected virtual IValue _RealValue { get; set; }

    protected RefValue()
    {
        
    }
    
    public RefValue(IValue val)
    {
        _RealValue = val;
    }
    
    public IValue Set(IValue newValue)
    {
        _RealValue = newValue;
        return _RealValue;
    }

    public IValue Get() => _RealValue;

    public string GetTypeName() => _RealValue.GetTypeName();

    public IValue Unpack()
    {
        if (_RealValue is RefValue realRefValue)
        {
            return realRefValue.Unpack();
        }
        else
        {
            return _RealValue;
        }
    }

    public bool ToBool() => _RealValue.ToBool();

    public Number ToNumber() => _RealValue.ToNumber();

    public IFunction ToFunction() => _RealValue.ToFunction();

    public SString ToSString() => _RealValue.ToSString();

    public ITable ToTable() => _RealValue.ToTable();

    public SFile ToSFile() => _RealValue.ToSFile();

    public bool IsEqual(IValue other) => _RealValue.IsEqual(other);

    public override string ToString() => _RealValue.ToString();

    public SString Serialize() => _RealValue.Serialize();
}