using System;
using Xunit;
using Moq;

namespace ShaellLang.Test.Unit;

public class ScopeContextUnitTest
{

    [Fact]
    public void GetValueOnEmptyScopeReturnsNull()
    {
        ScopeContext empty = new(); 
        
        var value = empty.GetValue("key");
        
        Assert.Null(value);
    }

    private IValue _mockValue = Mock.Of<IValue>();
    
    [Fact]
    public void GetValueReturnsValue()
    {
        ScopeContext context = new();
        context.NewValue("key", _mockValue);

        var value = context.GetValue("key");
        
        Assert.Same(value.Get(), _mockValue);
    }

    [Fact]
    public void NewValueReturnsValueAsRefValue()
    {
        ScopeContext context = new();

        var value = context.NewValue("key", _mockValue);
        
        Assert.Same(value.Get(), _mockValue);
    }

    [Fact]
    public void InsertingKeyTwiceThrows()
    {
        ScopeContext context = new();
        context.NewValue("key", _mockValue);
        
        var action = () =>
        {
            context.NewValue("key", _mockValue);
        };

        Assert.Throws<Exception>(action);
    }
    
}