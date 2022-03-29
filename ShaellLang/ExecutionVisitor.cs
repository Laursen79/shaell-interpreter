using System;
using System.Collections.Generic;

namespace ShaellLang;

public class ExecutionVisitor : ShaellBaseVisitor<IValue>
{
    private ScopeManager _scopeManager;
    private ScopeContext _globalScope;
    private bool _shouldReturn;
    public ExecutionVisitor()
    {
        _globalScope = new ScopeContext();
        _scopeManager = new ScopeManager();
        _scopeManager.PushScope(_globalScope);
        _shouldReturn = false;
    }
    
    public ExecutionVisitor(ScopeContext globalScope, ScopeManager scopeManager)
    {
        _globalScope = globalScope;
        _scopeManager = scopeManager;
        _shouldReturn = false;
    }

    public void SetGlobal(string key, IValue val)
    {
        _globalScope.SetValue(key, val);
    }
    
    public override IValue VisitProg(ShaellParser.ProgContext context)
    {
        VisitStmts(context.stmts());
        return null;
    }
    
    public override IValue VisitStmts(ShaellParser.StmtsContext context)
    {
        _scopeManager.PushScope(new ScopeContext());
        foreach (var stmt in context.stmt())
        {
            var rv = VisitStmt(stmt);
            if (_shouldReturn)
            {
                _shouldReturn = false;
                return rv;
            }
        }
        _scopeManager.PopScope();
        return null;
    }

    public override IValue VisitStmt(ShaellParser.StmtContext context)
    {
        if (context.children.Count == 1)
        {
            return Visit(context.children[0]);
        }
        else
        {
            throw new Exception("No no no");
        }
    }

    public override IValue VisitIfStmt(ShaellParser.IfStmtContext context)
    {
        var stmts = context.stmts();
        
        if (Visit(context.expr()).ToBool())
        {
            return VisitStmts(stmts[0]);
        }
        else if (stmts.Length > 1)
        {
            return VisitStmts(stmts[1]);
        }

        return null;
    }

    public override IValue VisitWhileLoop(ShaellParser.WhileLoopContext context)
    {
        while (Visit(context.expr()).ToBool())
        {
            var rv = VisitStmts(context.stmts());
            if (_shouldReturn)
            {
                return rv;
            }
        }

        return null;
    }

    public override IValue VisitReturnStatement(ShaellParser.ReturnStatementContext context)
    {
        _shouldReturn = true;
        //TODO: Kan returnere som reference
        return Visit(context.expr());
    }

    public override IValue VisitFunctionDefinition(ShaellParser.FunctionDefinitionContext context)
    {
        var formalArgIdentifiers = new List<string>();
        foreach (var formalArg in context.innerFormalArgList().VARIDENTFIER())
        {
            formalArgIdentifiers.Add(formalArg.GetText());
        }
        
        _scopeManager.SetValue(
            context.VARIDENTFIER().GetText(),
            new UserFunc(
                _globalScope, 
                context.stmts(), 
                _scopeManager.CopyScopes(), 
                formalArgIdentifiers
                )
        );
        
        return null;
    }

    public override IValue VisitExpr(ShaellParser.ExprContext context)
    {
        throw new Exception("nejnejnej");
    }
    
    public override IValue VisitAssignExpr(ShaellParser.AssignExprContext context)
    {
        var lhs = Visit(context.expr(0));
        
        if (lhs is not RefValue)
        {
            throw new Exception("Tried to assign to non ref");
        }

        RefValue refLhs = lhs as RefValue;

        var rhs = Visit(context.expr(1));
        if (rhs is RefValue)
        {
            rhs = (rhs as RefValue).Get();
        }
        //TODO: Det her assigner lhs til en reference til IValue hvilket lige skal fixes
        
        refLhs.Set(rhs);

        return refLhs.Get();
    }

    public override IValue VisitAddExpr(ShaellParser.AddExprContext context)
    {
        var lhs = Visit(context.expr(0));
        var rhs = Visit(context.expr(1));

        return lhs.ToNumber() + rhs.ToNumber();
    }

    public override IValue VisitVarIdentifier(ShaellParser.VarIdentifierContext context)
    {
        var val = _scopeManager.GetValue(context.VARIDENTFIER().GetText());
        if (val == null)
        {
            return _scopeManager.SetValue(context.VARIDENTFIER().GetText(), new SNull());
        }
        return val;
    }

    public override IValue VisitNumberExpr(ShaellParser.NumberExprContext context)
    {
        //TODO: Implement floating point or int parsing instead of just int
        
        return new Number(int.Parse(context.NUMBER().GetText()));
    }

    public override IValue VisitFunctionCallExpr(ShaellParser.FunctionCallExprContext context)
    {
        var lhs = Visit(context.expr()).ToFunction();
        
        var args = new List<IValue>();
        foreach (var expr in context.innerArgList().expr())
        {
            var val = Visit(expr);
            if (val is RefValue refVal)
            {
                val = refVal.Get();
            }

            args.Add(val);
        }

        return lhs.Call(args);
    }

    public override IValue VisitStringLiteralExpr(ShaellParser.StringLiteralExprContext context)
    {
        return new SString(context.STRINGLITERAL().GetText()[1..^1]);
    }
    
    //Visit DivExpr and evaluate both sides and return the two values divided
    public override IValue VisitDivExpr(ShaellParser.DivExprContext context)
    {
        var lhs = Visit(context.expr(0));
        var rhs = Visit(context.expr(1));

        return lhs.ToNumber() / rhs.ToNumber();
    }
    
    //Visit PosExpr and return the value with toNumber
    public override IValue VisitPosExpr(ShaellParser.PosExprContext context)
    {
        var lhs = Visit(context.expr());
        return lhs.ToNumber();
    }
    
    //Visit NegExpr and return the value with negative toNumber
    public override IValue VisitNegExpr(ShaellParser.NegExprContext context)
    {
        var lhs = Visit(context.expr());
        return -lhs.ToNumber();
    }
    
    //Visit the LORExpr and return the value of the left or right side with short circuiting
    public override IValue VisitLORExpr(ShaellParser.LORExprContext context)
    {
        var lhs = Visit(context.expr(0));
        if (lhs.ToBool())
            return new SBool(true);
        
        var rhs = Visit(context.expr(1));
        return new SBool(rhs.ToBool());
    }
    
    //Visit the IdentifierIndexExpr and use the right value to index the left as a table
    public override IValue VisitIdentifierIndexExpr(ShaellParser.IdentifierIndexExprContext context)
    {
        var lhs = Visit(context.expr());
        var rhs = context.identifier().GetText();

        return lhs.ToTable().GetValue(new SString(rhs));
    }
    
    //Visit the TrueBoolean and return the value of true
    public override IValue VisitTrueBoolean(ShaellParser.TrueBooleanContext context) => new SBool(true);
    
    //Visit the FalseBoolean and return the value of false
    public override IValue VisitFalseBoolean(ShaellParser.FalseBooleanContext context) => new SBool(false);
    
    //Vist the SubScriptExpr and return the value of the left side with the right side as index
    public override IValue VisitSubScriptExpr(ShaellParser.SubScriptExprContext context)
    {
        var lhs = Visit(context.expr(0));
        var rhs = Visit(context.expr(1));

        if (rhs is RefValue refValue)
        {
            rhs = refValue.Get();
        }

        if (rhs is IKeyable rhsKeyable)
        {
            return lhs.ToTable().GetValue(rhsKeyable);
        }
        
        throw new Exception("Cannot index with a non-keyable value");
    }
    
    //Implement less than operator
    public override IValue VisitLTExpr(ShaellParser.LTExprContext context)
    {
        var lhs = Visit(context.expr(0));
        var rhs = Visit(context.expr(1));

        return new SBool(lhs.ToNumber() < rhs.ToNumber());
    }
    
    //Implement DerefExpr
    public override IValue VisitDerefExpr(ShaellParser.DerefExprContext context) => new SFile(Visit(context.expr()));
    public override IValue VisitFileIdentifier(ShaellParser.FileIdentifierContext context) => new SFile(context.GetText());
}