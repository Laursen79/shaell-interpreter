using Xunit;

namespace ShaellLang.Test;

public class UnitTest
{
    [Fact]
    public void TestOperators()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        shaellLang.LoadTestLib();
        
        shaellLang.RunFile("../../../OperatorTest.æ");
        
        Assert.False(TestLib.testFailed);
    }
    
    [Fact]
    public void TestMetaTables()
    {   
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        shaellLang.LoadTestLib();

        shaellLang.RunFile("../../../MetatableTest.æ");

        Assert.False(TestLib.testFailed);
    }

    [Fact]
    public void TestStringInterpolation()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        shaellLang.LoadTestLib();

        shaellLang.RunFile("../../../StringInterpolationTest.æ");
        
        Assert.False(TestLib.testFailed);
    }
    
    [Fact]
    public void TestScope()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        shaellLang.LoadTestLib();

        shaellLang.RunFile("../../../ScopeTest.æ");
        
        Assert.False(TestLib.testFailed);
    }

    [Fact]
    public void TestForeach()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        shaellLang.LoadTestLib();

        shaellLang.RunFile("../../../ForeachTest.æ");
        
        Assert.False(TestLib.testFailed);
    }
    
    
    [Fact]
    public void TestTryThrow()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        shaellLang.LoadTestLib();

        shaellLang.RunFile("../../../TryThrowTest.æ");
        
        Assert.False(TestLib.testFailed);
    }

    [Fact]
    public void TestTypeSystem()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        shaellLang.LoadTestLib();

        shaellLang.RunFile("../../../TypeSystemTest.æ");

        Assert.False(TestLib.testFailed);
    }
    
    [Fact]
    public void TestDeref()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        shaellLang.LoadTestLib();

        shaellLang.RunFile("../../../DerefTest.æ");
        
        Assert.False(TestLib.testFailed);
    }

    [Fact]
    public void TestFileOperations()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        shaellLang.LoadTestLib();

        shaellLang.RunFile("../../../FileOperationsTest.æ");
        
        Assert.False(TestLib.testFailed);
    }

    [Fact]
    public void TestWhitespace()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        shaellLang.LoadTestLib();

        shaellLang.RunFile("../../../WhitespaceTest.æ");
        
        Assert.False(TestLib.testFailed);
    }
}