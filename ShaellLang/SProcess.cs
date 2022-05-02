#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ProcessLib;

namespace ShaellLang;

public class SProcess : BaseTable, IProcess
{
    private IEnumerable<string> _args;
    private ExternalProgram _externalProgram;
    public SString StoredOut { get; private set; }
    private SString _storedErr;
    private bool _hasBeenRun;
    public IReadStream Out { get; }
    public IReadStream Err { get; }
    public IWriteStream In { get; }
    public int ExitCode { get; }

    public SProcess(string filename, IEnumerable<string> args) : base("process")
    {
        _args = args;
        _externalProgram = new ExternalProgram(filename, args);
        _hasBeenRun = false;
        Out = _externalProgram.Out;
        Err = _externalProgram.Err;
        In = _externalProgram.In;
        SetValue((SString) "out", new SStream(In, Out, this));
        SetValue((SString) "err", new SStream(In, Err, this));
    }

    public async Task Run()
    {
        var outWriteStream = new StringWriteStream("");
        var errWriteStream = new StringWriteStream("");
        _externalProgram.Out.Pipe(outWriteStream);
        _externalProgram.Err.Pipe(errWriteStream);
        if (_hasBeenRun)
        {
            throw new Exception("Process has already been run");
        }
        
        await _externalProgram.Execute();
        _hasBeenRun = true;
        StoredOut = new SString(outWriteStream.Val);
        _storedErr = new SString(errWriteStream.Val);
    }

    public override ITable ToTable()
    {
        return this;
    }

    public override SProcess ToSProcess() => this;

    public override bool IsEqual(IValue other) => this == other;
    
}
