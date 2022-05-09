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
    private string _name;

    public UserFunc(
        ScopeContext globalScope, 
        ShaellParser.FunctionBodyContext funcBody, 
        ScopeManager capturedScope, 
        List<string> formalArguments,
        string name
        ) : base("userfunc")
    {
        _funcBody = funcBody;
        _capturedScope = capturedScope;
        _formalArguments = formalArguments;
        _name = name;
        _globalScope = globalScope;
    }

    public override bool ToBool() => true;

    public IValue Call(IEnumerable<IValue> args)
    {
        ScopeManager activeScopeManager = _capturedScope.CopyScopes();
        activeScopeManager.PushScope(new ScopeContext());
        var arr = args.ToArray();
        for (var i = 0; i < _formalArguments.Count; i++)
        {
            if (i < arr.Length)
                activeScopeManager.NewTopLevelValue(_formalArguments[i], arr[i]);
            else
                activeScopeManager.NewTopLevelValue(_formalArguments[i], new SNull());
        }

        var executioner = new ExecutionVisitor(_globalScope, activeScopeManager);

        return executioner.VisitFunctionBody(_funcBody, _name);
        //var jo = new JobObject(executioner.VisitFunctionBody(_funcBody));
        //return jo;
    }

    public uint ArgumentCount => 0;

    public override IFunction ToFunction() => this;
    
    public override bool IsEqual(IValue other) => other == this;
}