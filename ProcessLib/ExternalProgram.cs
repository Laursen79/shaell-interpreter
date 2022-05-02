using System.Diagnostics;

namespace ProcessLib;

public class ExternalProgram
{
    public event EventHandler ProcessExited;
    public event EventHandler ProcessStarted;
    //public Stream OutStream { get; set; } = new MemoryStream();
    //public Stream ErrStream { get; set; }
    //public Stream InStream { get; set; }
    
    private ProcessReadStream _out;
    private ProcessReadStream _err;
    private WriteStream _in;
    private Process _process;
    private bool _started;
    
    public ExternalProgram(string fileName, IEnumerable<string> arguments)
    {
        _process = new Process();
        _process.StartInfo.FileName = fileName;
        foreach (string argument in arguments)
        {
            _process.StartInfo.ArgumentList.Add(argument);
        }
        _process.StartInfo.UseShellExecute = false;
        _process.StartInfo.RedirectStandardInput = true;
        _process.StartInfo.CreateNoWindow = true;
        _process.EnableRaisingEvents = true;
        _process.StartInfo.RedirectStandardInput = true;
        _process.Exited += (sender, args) => ProcessExited?.Invoke(sender, args);
        _out = new ProcessReadStream();
        _err = new ProcessReadStream();
        _in = new WriteStream();

        _started = false;
    }

    /*public void PipeIn(Stream stream)
    {
        stream.Position = 0;
        InStream = stream;
        InStream.CopyTo(_process.StandardInput.BaseStream);
        _process.StandardInput.Close();
    }*/

    /*public Stream PipeOut()
    {
        OutStream.Position = 0;
        if (!_started)
            throw new Exception("Process not started");

        var s = _process.StandardOutput.ReadToEnd();
        new StreamWriter(OutStream).Write(s);

        return OutStream;
    }*/

    public async Task Execute()
    {
        Start();
        Wait();
    }
    
    public void Start()
    {
        if (_started)
        {
            throw new Exception("JobObject already started");
        }
        
        _process.StartInfo.RedirectStandardOutput = _out.RecipientCount > 0;
        _process.StartInfo.RedirectStandardError = _err.RecipientCount > 0;
        _process.Start();
        Console.WriteLine("Started process: " + _process.StartInfo.FileName);
        _started = true;
        if (_out.RecipientCount > 0)
        {
            _out.Bind(_process, _process.StandardOutput);
            _out.StartPiping();
        }

        if (_err.RecipientCount > 0)
        {
            _err.Bind(_process, _process.StandardError);
            _err.StartPiping();
        }
        _in.Bind(_process.StandardInput);
    }

    public void Wait()
    {
        _process.WaitForExit();
        if (_out.RecipientCount > 0)
            _out.WaitForFinish();
        if (_err.RecipientCount > 0)
            _err.WaitForFinish();
    }

    public IReadStream Out => _out;
    public IReadStream Err => _err;
    public IWriteStream In => _in;
}

public class ProcessOutput
{
    public IReadStream Out { get; }
    public IReadStream Err { get; }
    public int ExitCode { get; }

    public ProcessOutput(IReadStream @out, IReadStream err, int exitCode)
    {
        Out = @out;
        Err = err;
        ExitCode = exitCode;
    }
}