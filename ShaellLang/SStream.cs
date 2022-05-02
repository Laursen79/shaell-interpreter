using ProcessLib;

namespace ShaellLang;

public class SStream :BaseValue
{
    public SStream(IWriteStream @in, IReadStream @out, IProcess parent)
    {
        In = @in;
        Out = @out;
        Parent = parent;
    }

    public IReadStream Out { get; }
    public IWriteStream In { get; }
    public IProcess Parent { get; }
    public override bool IsEqual(IValue other)
    {
        throw new System.NotImplementedException();
    }
}