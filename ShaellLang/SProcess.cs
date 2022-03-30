using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Threading.Tasks;

namespace ShaellLang;

public class SProcess : IFunction
{
    private Process _process = new Process();
    public SProcess(string file)
    {
        _process.StartInfo.FileName = file;
    }

    private void AddArguments(ICollection<IValue> args)
    {
        foreach (var arg in args)
        {
            AddArg(arg.ToSString().Val);
        }
    }

    private void AddArg(string str) => _process.StartInfo.ArgumentList.Add(str);
    public void Dispose() => _process.Dispose();

    private JobObject Run(Process process)
    {
        return JobObject.Factory.StartProcess(process);
    }

    public IValue Call(ICollection<IValue> args)
    {
        AddArguments(args);
        return Run(_process);
    }
    
    IFunction IValue.ToFunction() => this;
    public bool ToBool() => throw new System.NotImplementedException();
    public Number ToNumber() => throw new System.NotImplementedException();
    public IFunction ToFunction => throw new System.NotImplementedException();
    public SString ToSString() => throw new System.NotImplementedException();
    public ITable ToTable() => throw new System.NotImplementedException();
    public uint ArgumentCount { get; }
}