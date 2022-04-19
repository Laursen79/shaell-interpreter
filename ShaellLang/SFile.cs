using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShaellLang;

public class SFile : BaseValue
{
    private string _path;
    private NativeTable _table;

    public SFile(string path) : base("file")
    {
        _path = path;
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

    private IValue ExistsFunc(IEnumerable<IValue> argCollection)
    {
        return new SBool(File.Exists(_path));
    }

    private IValue SizeFunc(IEnumerable<IValue> argCollection)
    {
        return new Number(new FileInfo(_path).Length);
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
        StreamWriter f = new FileInfo(_path).AppendText();
        f.WriteLine(argCollection.ToArray()[0].ToSString().Val);
        f.Flush();
        return new SNull();
    }

    private IValue ReadToEndFunc(IEnumerable<IValue> argCollection)
    {
        if (!File.Exists(_path)) throw new Exception("No file");
        return new BString(File.ReadAllText(_path));
    }

    private IValue DeleteFunc(IEnumerable<IValue> argCollection)
    {
        Console.WriteLine(_path);
        File.Delete(_path);
        return new SNull();
    }

    private IValue ReadFunc(IEnumerable<IValue> argCollection)
    {
        long[] args = argCollection.Select(y => y.ToNumber().ToInteger()).ToArray();
        FileStream fs = new FileInfo(_path).OpenRead();
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

    public override SString ToSString() => new (_path);

    public override ITable ToTable() => _table;
    public override bool IsEqual(IValue other)
    {
        return other == this;
    }
}