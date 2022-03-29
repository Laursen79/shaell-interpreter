using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class UserFunc : IFunction
{
    private ShaellParser.StmtsContext _funcStmts;
    private ScopeManager _capturedScope;
    private List<string> _formalArguments;
    private ScopeContext _globalScope;

    public UserFunc(
        ScopeContext globalScope, 
        ShaellParser.StmtsContext funcStmts, 
        ScopeManager capturedScope, 
        List<string> formalArguments
        )
    {
        _funcStmts = funcStmts;
        _capturedScope = capturedScope;
        _formalArguments = formalArguments;
        _globalScope = new ScopeContext();
    }

    public bool ToBool() => true;
    public Number ToNumber()
    {
        throw new Exception("Type error, function cannot be converted to number");
    }

    public IValue Call(ICollection<IValue> args)
    {
        ScopeManager activeScopeManager = _capturedScope.CopyScopes();
        activeScopeManager.PushScope(new ScopeContext());
        var arr = args.ToArray();
        for (var i = 0; i < args.Count && i < _formalArguments.Count; i++)
        {
            activeScopeManager.SetValue(_formalArguments[i], arr[i]);
        }

        var executioner = new ExecutionVisitor(_globalScope, activeScopeManager);

        var jo = new JobObject(executioner.VisitStmts(_funcStmts));

        return jo;
    }

    public uint ArgumentCount => 0;

    public IFunction ToFunction()
    {
        return this;
    }

    public SString ToSString()
    {
        throw new Exception("Type error, function cannot be converted to a string");
    }

    public ITable ToTable()
    {
        throw new Exception("Type error: function cannot be converted to table");
    }
}