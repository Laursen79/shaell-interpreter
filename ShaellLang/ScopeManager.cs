using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class ScopeManager
{
    private List<ScopeContext> _scopes;

    public ScopeManager()
    {
        _scopes = new List<ScopeContext>();
    }

    public ScopeManager CopyScopes()
    {
        ScopeManager rv = new ScopeManager();
        foreach (var scope in _scopes)
        {
            rv.PushScope(scope);
        }

        return rv;
    }
    
    public RefValue GetValue(string key)
    {
        foreach (var scope in _scopes)
        {
            var rv = scope.GetValue(key);
            if (rv != null)
            {
                return rv;
            }
        }

        return null;
    }

    public RefValue SetValue(string key, IValue val)
    {
        return _scopes.Last().SetValue(key, val);
    }

    public void PushScope(ScopeContext scopeContext)
    {
        _scopes.Add(scopeContext);
    }

    public void PopScope()
    {
        if (_scopes.Count > 0)
        {
            _scopes.RemoveAt(_scopes.Count - 1);
        }

    }
}