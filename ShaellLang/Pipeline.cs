using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProcessLib;

namespace ShaellLang
{
    internal class Pipeline : BaseTable, IPipeline
    {
        public List<IPipeable> Steps { get; set; } = new List<IPipeable>();
        public Pipeline(IPipeable process, IPipeable nextProcess):base("pipeline")
        {
            Steps.Add(process);
            Steps.Add(nextProcess);
            SetValue(new SString("run"), new NativeFunc(RunFunc, 0));
        }
        public IValue RunFunc(IEnumerable<IValue> args)
        {
            var task = Run();
            task.Wait();
            return null;
        }

        public IReadStream Out => Steps.Last().Out;
        public IReadStream Err => Steps.Last().Err;
        public IWriteStream In => Steps.First().In;
        
        //TODO: What is the exit code of a pipeline?
        public int ExitCode => (Steps.Last() is IProcess process)?process.ExitCode : 0;

        public async Task Run()
        {
            foreach (var pipeable in Steps)
            {
                if (pipeable is IProcess process)
                {
                    string @out = "";
                    string @out2 = "";
                    var writer = new StringWriteStream(@out);
                    var writer2 = new StringWriteStream(@out2);
                    process.Out.Pipe(writer);
                    process.Err.Pipe(writer2);
                    await process.Run();
                    Console.WriteLine(writer.Val);
                    Console.WriteLine(writer2.Val);
                }
            }
        }
    }

    public interface IPipeline : IProcess, IValue
    {
        
    }
    
}
