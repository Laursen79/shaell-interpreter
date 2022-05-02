
using System;

namespace ShaellLang;

public interface IPipeServer
{   
    public void Start();
    event EventHandler PipelineReady;
}