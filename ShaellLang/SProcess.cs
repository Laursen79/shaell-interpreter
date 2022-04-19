using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ProcessLib;

namespace ShaellLang;

public class SProcess
    : BaseValue
{
    private IEnumerable<string> _args;
    private ExternalProgram _externalProgram;
    private SString _storedOut;
    private SString _storedErr;
    private bool _hasBeenRun;
    public IReadStream Out { get; }
    public IReadStream Err { get; }
    public IWriteStream In { get; }
    
    public SProcess(string filename, IEnumerable<string> args) 
        : base("sprocess")
    {
        _args = args;
        _externalProgram = new ExternalProgram(filename, args);
        _hasBeenRun = false;
        Out = _externalProgram.Out;
        Err = _externalProgram.Err;
        In = _externalProgram.In;
    }

    public void Run()
    {
        var outWriteStream = new StringWriteStream("");
        var errWriteStream = new StringWriteStream("");
        _externalProgram.Out.Pipe(outWriteStream);
        _externalProgram.Err.Pipe(errWriteStream);
        _externalProgram.Start();
        _externalProgram.Wait();
        _storedOut = new SString(outWriteStream.Val);
        _storedErr = new SString(errWriteStream.Val);
        _hasBeenRun = true;
    }

    public override ITable ToTable()
    {
        if (!_hasBeenRun)
            Run();
        var table = new BaseTable("Process");
        
        table.SetValue(new SString("out"), _storedOut);
        table.SetValue(new SString("err"), _storedErr);
        
        return table;
    }

    public override SProcess ToSProcess() => this;

    public override bool IsEqual(IValue other) => this == other;
}