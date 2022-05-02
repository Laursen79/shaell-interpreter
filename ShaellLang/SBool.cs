using System;

namespace ShaellLang;

public class SBool : BaseValue
{
    private bool _value;
    public SBool(bool value)
    {
        _value = value;
    }


    public override bool ToBool() => _value;
    public override Number ToNumber() => _value ? new Number(1) : new Number(0);

    public override SString ToSString() => new SString(_value ? "true" : "false");

    public override bool IsEqual(IValue other)
    {
        return other.ToBool() == _value;
    }
}