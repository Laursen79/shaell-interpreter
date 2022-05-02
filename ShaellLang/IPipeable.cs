using System.IO;
using System.Threading.Tasks;
using ProcessLib;

namespace ShaellLang;

public interface IPipeable
{
    IReadStream Out {get;}
    IReadStream Err {get;}
    IWriteStream In {get;}
}