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
        _process.EnableRaisingEvents = true;
        _process.StartInfo.UseShellExecute = true;
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
    
    public IValue Call(ICollection<IValue> args)
    {
        AddArguments(args);
        _process.Start();
        return this;
        /*var jo = JobObject.Factory.ProcessCall(() =>
        {
            int x = 0;
            for (int i = 0; i < 1000; i++)
            {
                x += i + 3;
            }

            return x;
        });
        return jo;*/
    }
    
    IFunction IValue.ToFunction() => this;
    public bool ToBool() => throw new System.NotImplementedException();
    public Number ToNumber() => throw new System.NotImplementedException();
    public IFunction ToFunction => throw new System.NotImplementedException();
    public SString ToSString() => throw new System.NotImplementedException();
    public ITable ToTable() => throw new System.NotImplementedException();
    public uint ArgumentCount { get; }
}