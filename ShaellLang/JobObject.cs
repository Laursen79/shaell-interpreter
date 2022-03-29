using System;
using System.Collections.Generic;
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

    public StreamReader In { get; set; }
    public StreamWriter Out { get; set; }
    public StreamReader Error { get; set; }

    private Task<int> _process;

    public JobObject(IValue value)
    {
        SetValue("value", value);
    }

    private JobObject(){}

    public static class Factory
    {
        public static JobObject ProcessCall(Func<ICollection<IValue>, int> process, ICollection<IValue> args)
        {
            var func = new Func<object, int>(values => process.Invoke(values as ICollection<IValue>));
            var jo = new JobObject
            {
                _process = Task.Factory.StartNew(func, args)
            };
            jo.Value = new Number(jo._process.Result);
            
            return jo;
        }
        public static JobObject ProcessCall(Func<int> process)
        {
            var jo = new JobObject
            {
                _process = Task.Factory.StartNew(process)
            };
            jo.Value = new Number(jo._process.Result);
            
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