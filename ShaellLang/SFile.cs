using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProcessLib;

namespace ShaellLang;

public class SFile : BaseValue, IPipeable
{
    private string _path;
    private NativeTable _table;
    private string _cwd;
    public SFile(string path, string cwd) : base("file")
    {
        _path = path;
        _cwd = cwd;
        GenerateFileValues();
    }
    
    private void GenerateFileValues()
    {
        _table = new NativeTable();
        _table.SetValue("read", new NativeFunc( ReadFunc, 2));
        _table.SetValue("delete", new NativeFunc( DeleteFunc, 0));
        _table.SetValue("readToEnd", new NativeFunc( ReadToEndFunc, 0));
        _table.SetValue("append", new NativeFunc( AppendFunc, 1));
        _table.SetValue("openReadStream", new NativeFunc( OpenReadStreamFunc, 0));
        _table.SetValue("openWriteStream", new NativeFunc( OpenWriteSteamFunc, 0));
        _table.SetValue("size", new NativeFunc( SizeFunc, 2));
        _table.SetValue("exists", new NativeFunc( ExistsFunc, 0));
    }

    private string RealPath => Path.Join(_cwd, _path);

    private IValue ExistsFunc(IEnumerable<IValue> argCollection)
    {
        return new SBool(File.Exists(RealPath));
    }

    private IValue SizeFunc(IEnumerable<IValue> argCollection)
    {
        return new Number(new FileInfo(RealPath).Length);
    }

    private IValue OpenWriteSteamFunc(IEnumerable<IValue> argCollection)
    {
        throw new NotImplementedException();
    }

    private IValue OpenReadStreamFunc(IEnumerable<IValue> argCollection)
    {
        throw new NotImplementedException();
    }

    private IValue AppendFunc(IEnumerable<IValue> argCollection)
    {
        StreamWriter f = new FileInfo(RealPath).AppendText();
        f.WriteLine(argCollection.ToArray()[0].ToSString().Val);
        f.Flush();
        return new SNull();
    }

    private IValue ReadToEndFunc(IEnumerable<IValue> argCollection)
    {
        if (!File.Exists(RealPath)) throw new Exception("No file");
        return new BString(File.ReadAllText(_path));
    }

    private IValue DeleteFunc(IEnumerable<IValue> argCollection)
    {
        File.Delete(RealPath);
        return new SNull();
    }

    private IValue ReadFunc(IEnumerable<IValue> argCollection)
    {
        long[] args = argCollection.Select(y => y.ToNumber().ToInteger()).ToArray();
        FileStream fs = new FileInfo(RealPath).OpenRead();
        fs.Seek(args[1], SeekOrigin.Begin);

        byte[] buffer = new byte[args[0]];
        fs.Read(buffer, 0, (int) args[0]);

        return new BString(buffer);
    }
    
    public StreamWriter OpenWriteStream(bool append)
    {
        return new StreamWriter(_path, append);
    }

    public override bool ToBool() => true;

    public override Number ToNumber() => throw new Exception("Type error, File cannot be convert to number.");

    public override IFunction ToFunction() => new SProcessFuncWrap(_path);

    public override SString ToSString() => new SString($"(FilePath \"{_path}\" relative to \"{_cwd}\")");

    public override ITable ToTable() => _table;
    
    public override bool IsEqual(IValue other)
    {
        return other == this;
    }

    private IReadStream _out = null;
    private IWriteStream _in = null;
    public IReadStream Out => _out ??= new StringReadStream(File.ReadAllText(_path));

    public IReadStream Err => throw new NotImplementedException();
    public IWriteStream In => _in ??= new WriteStream(this.OpenWriteStream(true));
}