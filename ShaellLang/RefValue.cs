namespace ShaellLang;

public class RefValue : IValue
{
    private IValue _realValue;
    public RefValue(IValue val)
    {
        _realValue = val;
    }
    
    public IValue Set(IValue newValue)
    {
        _realValue = newValue;
        return _realValue;
    }

    public IValue Get() => _realValue;

    public string GetTypeName() => _realValue.GetTypeName();

    public IValue Unpack()
    {
        if (_realValue is RefValue realRefValue)
        {
            return realRefValue.Unpack();
        }
        else
        {
            return _realValue;
        }
    }

    public bool ToBool() => _realValue.ToBool();

    public Number ToNumber() => _realValue.ToNumber();

    public IFunction ToFunction() => _realValue.ToFunction();

    public SString ToSString() => _realValue.ToSString();

    public ITable ToTable() => _realValue.ToTable();

    public SFile ToSFile() => _realValue.ToSFile();

    public bool IsEqual(IValue other) => _realValue.IsEqual(other);

    public override string ToString() => _realValue.ToString();

    public SString Serialize() => _realValue.Serialize();
}