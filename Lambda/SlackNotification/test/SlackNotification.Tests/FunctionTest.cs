using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

namespace SlackNotification.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {

        // Invoke the lambda function and confirm the string was upper cased.
        var function = new SlackNotification();
        var context = new TestLambdaContext();
        var result = function.SendSlackNotification("test", context);

        Assert.Equal("test", result);
    }
}
