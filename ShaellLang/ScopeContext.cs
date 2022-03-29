using System.Collections.Generic;

namespace ShaellLang;

public class ScopeContext
{
    private Dictionary<string, RefValue> _values;

    public ScopeContext()
    {
        _values = new Dictionary<string, RefValue>();
    }
    
    public RefValue GetValue(string key)
    {
        bool found = _values.TryGetValue(key, out RefValue foundValue);

        return found ? foundValue : null;
    }

    public RefValue SetValue(string key, IValue val)
    {
        RefValue rv = new RefValue(val);
        _values[key] = rv;
        return rv;
    }
}