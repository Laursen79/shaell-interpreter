using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ProcessLib;

namespace ShaellLang;

public class SProcessFuncWrap
    : BaseValue, IFunction
{
    private string _path;
    
    public SProcessFuncWrap(string path)
    {
        _path = path;
    }

    public override bool IsEqual(IValue other) => this == other;

    public IValue Call(IEnumerable<IValue> args)
    {
        var proc = new SProcess(_path, args.Select(x => x.ToSString().Val));
        var task = proc.Run();
        task.Wait();
        
        
        string o ="";
        /*using (StreamReader streamReader = new StreamReader(proc.OutStream))
        {
            proc.OutStream.Position = 0;
            o = streamReader.ReadToEnd();
        }*/

        Console.Out.Write(o);
        return proc;
    }

    public SProcess GetProcess(IEnumerable<IValue> args) => new SProcess(_path, args.Select(x => x.ToSString().Val));

    public uint ArgumentCount { get; }
}