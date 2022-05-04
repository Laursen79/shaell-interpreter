using Xunit;

namespace ShaellLang.Test;

public class UnitTest1
{
    [Fact]
    public void TestOperators()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        
        shaellLang.RunFile("../../../OperatorTest.æ");
        
        Assert.False(TestLib.testFailed);
    }
    
    [Fact]
    public void TestMetaTables()
    {   
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();

        shaellLang.RunFile("../../../MetatableTest.æ");

        Assert.False(TestLib.testFailed);
    }

    [Fact]
    public void TestStringInterpolation()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();

        shaellLang.RunFile("../../../StringInterpolationTest.æ");
        
        Assert.False(TestLib.testFailed);
    }
    
    [Fact]
    public void TestScope()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();

        shaellLang.RunFile("../../../ScopeTest.æ");
        
        Assert.False(TestLib.testFailed);
    }

    [Fact]
    public void TestForeach()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();

        shaellLang.RunFile("../../../ForeachTest.æ");
        
        Assert.False(TestLib.testFailed);
    }
    
    
    [Fact]
    public void TestTryThrow()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();

        shaellLang.RunFile("../../../TryThrowTest.æ");
        
        Assert.False(TestLib.testFailed);
    }
}