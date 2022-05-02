using System.Threading.Tasks;
using ProcessLib;

namespace ShaellLang
{
    public interface IProcess: IPipeable
    {
        int ExitCode { get; }

        Task Run();
    }
}