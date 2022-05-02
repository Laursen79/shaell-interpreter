namespace ProcessLib;

public class WriteStream : IWriteStream
{
    private StreamWriter _writer;
    private string _bufferedText;
    private bool _shouldClose;
    
    public WriteStream(StreamWriter writeStream)
    {
        _writer = writeStream;
        _bufferedText = "";
    }
    
    public WriteStream()
    {
        _writer = null;
        _bufferedText = "";
    }
    
    public void Bind(StreamWriter writer)
    {
        _writer = writer;
        if (_bufferedText.Length > 0)
        {
            _writer.Write(_bufferedText);
            _writer.Close();
        }
        //If everything was written before this was binding it should just immidiatly close
        if (_shouldClose)
        {
            _writer.Close();
        }
    }
    
    public void Write(string text)
    {

        if (_writer != null)
        {
            _writer.Write(text);
        }
        else
        {
            _bufferedText += text;
        }
    }

    public bool IsOpen()
    {
        if (_writer == null)
        {
            return true;
        }
        return _writer.BaseStream.CanWrite;
    }

    public void Close()
    {
        _shouldClose = true;
        if (_writer != null)
        {
            _writer.Close();
        }
    }
}