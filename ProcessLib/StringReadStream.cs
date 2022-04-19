namespace ProcessLib;

public class StringReadStream
    : IReadStream
{
    private readonly string _str;
    private bool _finished;
    private List<IWriteStream> _targets;
    
    public StringReadStream(string str)
    {
        _str = str;
        _targets = new List<IWriteStream>();
        _finished = false;
    }
    
    public void StartPiping()
    {
        foreach (var target in _targets)
        {
            target.Write(_str);
        }

        _finished = true;
        ReadStreamFinished?.Invoke();
    }

    public void Pipe(IWriteStream writeStream)
    {
        _targets.Add(writeStream);
    }

    public bool ReachedEnd()
    {
        return _finished;
    }

    public event ReadStreamFinished? ReadStreamFinished;
    public int RecipientCount => _targets.Count;
}