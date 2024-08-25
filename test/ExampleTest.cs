namespace Examples;

using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public class ExampleTest
{
    //TODO remove those example test and replace them with actual ones
    [TestCase]
    public void success()
    {
        AssertBool(true).IsTrue();
    }

    [TestCase]
    public void failed()
    {
        AssertBool(false).IsFalse();
    }
    
    [TestCase]
    public void demo()
    {
        AssertBool(false).IsTrue();
    }
}