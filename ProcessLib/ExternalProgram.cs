using System.Diagnostics;

namespace ProcessLib;

public class ExternalProgram
{
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
        
        _out = new ProcessReadStream();
        _err = new ProcessReadStream();
        _in = new WriteStream();

        _started = false;
    }

    public void Start()
    {
        if (_started)
        {
            throw new Exception("JobObject already started");
        }
        _started = true;
        _process.StartInfo.RedirectStandardOutput = _out.RecipientCount > 0;
        _process.StartInfo.RedirectStandardError = _err.RecipientCount > 0;
        _process.Start();
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