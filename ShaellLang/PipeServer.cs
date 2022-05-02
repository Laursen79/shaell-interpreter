using System;
using ProcessLib;

namespace ShaellLang;

public delegate void ExecutePipeline();

public class PipeServer : IPipeServer
{
    IPipeable lhs;
    IPipeable rhs;
    IReadStream leftStream;
    IWriteStream rightStream;

    public event EventHandler PipelineReady;

    public PipeServer(IPipeable lhs, IPipeable rhs)
    {
        this.lhs = lhs;
        this.rhs = rhs;
    }


    public void Start(){
        PipeOut();
        
        PipeIn();
    }

    private void PipeOut()
    {
        if (lhs is SString str)
            leftStream = new StringReadStream(str.Val);
        else if (lhs is IProcess proc)
        {
            proc.Run();
            leftStream = proc.Out;
        }
        else
            throw new Exception($"Cannot pipe {lhs.GetType()} as read stream");
    }

    private void PipeIn()
    {
        if (rhs is SFile sfile)
            rightStream = new WriteStream(sfile.OpenWriteStream(true));
        else if (rhs is IProcess proc2)
            rightStream = proc2.In;
        else if (rhs is SString str2)
            rightStream = new StringWriteStream(str2.Val);
        else
            throw new Exception($"Cannot use {rhs.GetType()} as write stream");
        
        leftStream.Pipe(rightStream);
        PipelineReady?.Invoke(this, EventArgs.Empty);
    }


     
}