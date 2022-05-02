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

    //public Stream OutStream => _externalProgram.OutStream;

    public SProcess(string filename, IEnumerable<string> args) 
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

    /*public async Task<ProcessOutput> Execute(Task<ProcessOutput>? previous)
    {
        return await Task.Run(async () =>
        {
            var outWriteStream = new StringWriteStream("");
            var errWriteStream = new StringWriteStream("");
            _externalProgram.Out.Pipe(outWriteStream);
            _externalProgram.Err.Pipe(errWriteStream);
            if (_hasBeenRun)
            {
                throw new Exception("Process has already been run");
            }

            var output = await _externalProgram.Execute(previous);
            _hasBeenRun = true;
            _storedOut = new SString(outWriteStream.Val);
            _storedErr = new SString(errWriteStream.Val);
            return output;
        });
    }*/

    public void PipeIn(Stream pipeOut)
    {
        throw new NotImplementedException();
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

    /*public void Run()
    {
        // var outWriteStream = new StringWriteStream("");
        // var errWriteStream = new StringWriteStream("");
        // _externalProgram.Out.Pipe(outWriteStream);
        // _externalProgram.Err.Pipe(errWriteStream);
        _externalProgram.ProcessExited += (sender, args) =>
        {
            OnComplete?.Invoke();
        };
        _externalProgram.Start();
        OnStart?.Invoke();
        _externalProgram.Wait();
        // _storedOut = new SString(outWriteStream.Val);
        // _storedErr = new SString(errWriteStream.Val);
        _hasBeenRun = true;
    }*/
    

    public override ITable ToTable()
    {
        return this;
        /*if (!_hasBeenRun)
            Run();
        var table = new BaseTable();
        
        table.SetValue(new SString("out"), StoredOut);
        table.SetValue(new SString("err"), _storedErr);
        
        return table;*/
    }

    public override SProcess ToSProcess() => this;

    public override bool IsEqual(IValue other) => this == other;
    /*public IPipeline PipeInto(IPipeable rhspipeable)
    {
        var pipe = new Pipeline(this);
        Console.WriteLine("Piping into process");
        _externalProgram.ProcessExited += (sender, args) =>
        {
            Console.WriteLine("Process exited");
            rhspipeable.OnStart += () => rhspipeable.PipeIn(PipeOut());
            
        };
        //pipe.Add(rhspipeable);
        return pipe;
    }*/

    public IPipeline PipeInto(IPipeable rhspipeable)
    {
        throw new NotImplementedException();
    }

    /*public void PipeIn(Stream pipeOut)
    {
        _externalProgram.PipeIn(pipeOut);
    }

    public Stream PipeOut()
    {
        return _externalProgram.PipeOut();
    }*/
}
