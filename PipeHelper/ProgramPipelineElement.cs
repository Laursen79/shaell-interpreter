using System.Diagnostics;

namespace PipeHelper;

public class ProgramPipelineElement
{
    private string _executablePath;
    private Process _process;
    private Dictionary<string, string> _capturedInput;
    
    public ProgramPipelineElement(string executablePath)
    {
        _executablePath = executablePath;
        _process = new Process();
        _process.StartInfo.FileName = executablePath;
        _process.StartInfo.UseShellExecute = false;
        _capturedInput = new Dictionary<string, string>();
    }
    
    public void CaptureOutput(string outName)
    {
        if (outName == "out")
        {
            _process.StartInfo.RedirectStandardOutput = true;
        }
        else if (outName == "err")
        {
            _process.StartInfo.RedirectStandardError = true;
        }
        
        _capturedInput.Add(outName, "");
    }

    public string GetOutput(string outputName)
    {
        if (!_capturedInput.TryGetValue(outputName, out string rv))
        {
            throw new Exception($"{outputName} was not captured");
        }
        return rv;
    }

    public int Run(IEnumerable<string> arguments, string? input)
    {
        foreach (var argument in arguments)
        {
            _process.StartInfo.ArgumentList.Add(argument);   
        }

        if (input != null)
            _process.StartInfo.RedirectStandardInput = true;

        _process.Start();
        if (input != null)
        {
            _process.StandardInput.Write(input);
            _process.StandardInput.Flush();
            _process.StandardInput.Close();
        }
        _process.WaitForExit();
        if (_capturedInput.ContainsKey("out"))
            _capturedInput["out"] = _process.StandardOutput.ReadToEnd();
        if (_capturedInput.ContainsKey("err"))
            _capturedInput["err"] = _process.StandardError.ReadToEnd();

        return _process.ExitCode;
    }
}