using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class SProcessFuncWrap
    : BaseValue, IFunction
{
    private string _path;
    
    public SProcessFuncWrap(string path) : base("processfuncwrap")
    {
        _path = path;
    }

    public override bool IsEqual(IValue other) => this == other;

    public IValue Call(IEnumerable<IValue> args)
    {
        var proc = new SProcess(_path, args.Select(x => x.ToSString().Val));
        proc.Run();
        return proc;
    }

    public SProcess GetProcess(IEnumerable<IValue> args) => new SProcess(_path, args.Select(x => x.ToSString().Val));

    public uint ArgumentCount { get; }
}