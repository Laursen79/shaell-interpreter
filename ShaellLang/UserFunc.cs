using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class UserFunc : BaseValue, IFunction
{
    private ShaellParser.FunctionBodyContext _funcBody;
    private ScopeManager _capturedScope;
    private List<string> _formalArguments;
    private ScopeContext _globalScope;

    public UserFunc(
        ScopeContext globalScope, 
        ShaellParser.FunctionBodyContext funcBody, 
        ScopeManager capturedScope, 
        List<string> formalArguments
        ) : base("userfunc")
    {
        _funcBody = funcBody;
        _capturedScope = capturedScope;
        _formalArguments = formalArguments;
        _globalScope = globalScope;
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

        return executioner.VisitFunctionBody(_funcBody);
        //var jo = new JobObject(executioner.VisitFunctionBody(_funcBody));
        //return jo;
    }

    public uint ArgumentCount => 0;

    public override IFunction ToFunction() => this;
    
    public override bool IsEqual(IValue other) => other == this;
}