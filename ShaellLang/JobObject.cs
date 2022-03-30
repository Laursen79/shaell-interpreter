using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ShaellLang;

public class JobObject : NativeTable, IValue
{
    private IValue Value
    {
        get => GetValue(new SString("value"));
        set => SetValue("value", value);
    }

    private IValue Out
    {
        get => GetValue(new SString("out"));
        set => SetValue("out", value);
    }
    
    private IValue Err
    {
        get => GetValue(new SString("err"));
        set => SetValue("err", value);
    }

    private readonly Process _process;

    public JobObject(IValue value) : this()
    {
        SetValue("value", value);
    }

    private JobObject(Process process) : this()
    {
        _process = process;
    }

    private JobObject()
    {
        SetValue("out", new SNull());
        SetValue("value", new SNull());
        SetValue("err", new SNull());
    }

    public static class Factory
    {
        public static JobObject StartProcess(Process process)
        {
            var jo = new JobObject(process);
            process.Exited += (sender, args) => jo.Value = new Number(process.ExitCode);
            process.EnableRaisingEvents = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();
            process.WaitForExit();
            jo.Out = new SString(process.StandardOutput.ReadToEnd());
            jo.Err = new SString(process.StandardError.ReadToEnd());

            return jo;
        }
    }

    public bool ToBool()
    {
        return Value.ToBool();
    }

    public Number ToNumber()
    {
        return Value.ToNumber();
    }

    public IFunction ToFunction()
    {
        return Value.ToFunction();
    }

    public SString ToSString()
    {
        return Value.ToSString();
    }

    public ITable ToTable()
    {
        return this;
    }
}