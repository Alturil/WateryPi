using Xunit;
using Amazon.Lambda.TestUtilities;
using System.Net;
using dotenv.net;
using Moq;

namespace SlackNotification.Tests;

public class SlackNotificationTest
{
    public SlackNotificationTest()
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SLACK_URI")))
        {
            LoadEnvFile();
        }
    }

    [Fact]
    public async Task When_CallingSendSlackNotification_Then_ReturnsOk()
    {
        // Arrange
        var mockSlackClient = new Mock<ISlackClient>();
        mockSlackClient.Setup(c => c.SendTapEventAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage(statusCode: HttpStatusCode.OK));

        var function = new SlackNotification(mockSlackClient.Object);
        var context = new TestLambdaContext();
        var result = await function.SendSlackNotification("test", context);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    private void LoadEnvFile()
    {
        var envFileLocation = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.env"));
        DotEnv.Load(options: new DotEnvOptions(
                ignoreExceptions: false,
                envFilePaths: new[] { envFileLocation }
            ));        
    }
}
