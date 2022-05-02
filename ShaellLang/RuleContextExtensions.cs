using System;
using Antlr4.Runtime;

namespace ShaellLang;

public static class RuleContextExtensions
{
    /// <summary>
    /// Returns the first ancestor of this rule context that has the specified type.
    /// </summary>
    /// <param name="context">The context to search.</param>
    /// <typeparam name="T">The type of context to search for.</typeparam>
    /// <returns>The First ancestor of type <typeparamref name="T"/></returns>
    public static T FindAncestorOfType<T>(this RuleContext context) where T : RuleContext
    {
        RuleContext ancestor = context.Parent;
        while (!ancestor.IsEmpty)
        {
            if (ancestor is T typedAncestor)
            {
                return typedAncestor;
            }
            ancestor = ancestor.Parent;
        }

        return null;
    }
    
    /// <summary>
    /// Returns whether the specified context has an ancestor of the specified type.
    /// </summary>
    /// <param name="context">The context to search.</param>
    /// <typeparam name="T">The type of context to search for.</typeparam>
    /// <returns></returns>
    public static bool GetHasAncestorOfType<T>(this RuleContext context) where T : RuleContext => FindAncestorOfType<T>(context) != null;
}