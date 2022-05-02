using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class UserFunc : BaseValue, IFunction
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

    public override bool ToBool() => true;

    public IValue Call(IEnumerable<IValue> args)
    {
        ScopeManager activeScopeManager = _capturedScope.CopyScopes();
        activeScopeManager.PushScope(new ScopeContext());
        var arr = args.ToArray();
        for (var i = 0; i < arr.Length && i < _formalArguments.Count; i++)
        {
            activeScopeManager.NewTopLevelValue(_formalArguments[i], arr[i]);
        }

        var executioner = new ExecutionVisitor(_globalScope, activeScopeManager);
        
        return executioner.VisitStmts(_funcStmts);
    }

    public uint ArgumentCount => 0;

    public override IFunction ToFunction()
    {
        return this;
    }
    
    public override bool IsEqual(IValue other)
    {
        return other == this;
    }
}