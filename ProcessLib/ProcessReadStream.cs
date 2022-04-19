using System.Diagnostics;

namespace ProcessLib;

public class ProcessReadStream : IReadStream
{
    private Process _process;
    private List<IWriteStream> _recepients;
    private StreamReader _associatedReader;
    private ProcessReadStreamServer _server;
    public event ReadStreamFinished ReadStreamFinished;
    public int RecipientCount => _recepients.Count;

    private class ProcessReadStreamServer
    {
        private readonly ProcessReadStream _stream;
        private bool _started;
        private Thread _thread;
        private bool _shouldStop;
        public ProcessReadStreamServer(ProcessReadStream stream)
        {
            _stream = stream;
            _started = false;
            _shouldStop = false;
        }

        public void StartServer()
        {
            if (_started)
            {
                throw new Exception("Server already started");
            }

            _started = true;

            _thread = new Thread(ServerThread);
            _thread.Start();
        }

        public void StopServer()
        {
            _shouldStop = true;
            _thread.Join();
        }

        private void ServerThread()
        {
            while (true)
            {
                string readText = _stream._associatedReader.ReadToEnd();
                if (readText.Length > 0)
                {
                    //Maybe this should be dispatched to the main thread to ensure thread safety
                    _stream.SendToRecipient(readText);
                }

                if (_stream._process.HasExited)
                {
                    break;
                }
            }
            _stream.CloseRecipients();
            _stream.ReadStreamFinished?.Invoke();
        }

        public void WaitForFinish()
        {
            _thread.Join();
        }
    }
    
    public ProcessReadStream()
    {
        _process = null;
        _recepients = new List<IWriteStream>();
        _associatedReader = null;

    }

    public void Bind(Process process, StreamReader associatedReader)
    {
        _process = process;
        _associatedReader = associatedReader;
    }

    public void StartPiping()
    {
        if (_server != null)
        {
            throw new Exception("Server already started");
        }
        _server = new ProcessReadStreamServer(this);
        _server.StartServer();
    }

    private void CloseRecipients()
    {
        foreach (var recipient in _recepients)
        {
            recipient.Close();
        }
    }

    public void Pipe(IWriteStream writeStream)
    {
        _recepients.Add(writeStream);
    }

    public bool ReachedEnd()
    {
        return _process.HasExited;
    }

    public void WaitForFinish()
    {
        _server.WaitForFinish();
    }
    
    private void SendToRecipient(string data)
    {
        foreach (var recipient in _recepients)
        {
            if (recipient.IsOpen())
            {
                recipient.Write(data);
            }
        }
        _recepients.RemoveAll(x => !x.IsOpen());
        if (_recepients.Count == 0)
        {
            _server.StopServer();
        }
    }
}