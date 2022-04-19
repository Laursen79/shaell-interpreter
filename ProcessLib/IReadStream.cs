namespace ProcessLib;


public delegate void ReadStreamFinished();
public interface IReadStream
{
    public void StartPiping();
    
    public void Pipe(IWriteStream writeStream);

    public bool ReachedEnd();

    public event ReadStreamFinished ReadStreamFinished;
    
    public int RecipientCount { get; }
}