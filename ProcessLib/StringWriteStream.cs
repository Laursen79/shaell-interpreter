namespace ProcessLib;

public class StringWriteStream : IWriteStream
{
    private string _str;
    public string Val
    {
        get
        {
            return _str;
        }
    }

    private bool _isClosed;
    public StringWriteStream(string str)
    {
        _str = str;
        _isClosed = false;
    }

    public void Write(string str)
    {
        if (_isClosed)
            throw new Exception("Stream is closed");
        _str += str;
    }

    public bool IsOpen()
    {
        return !_isClosed;
    }

    public void Close()
    {
        _isClosed = true;
        return;
    }
}