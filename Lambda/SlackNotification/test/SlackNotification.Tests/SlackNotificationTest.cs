using Xunit;
using Amazon.Lambda.TestUtilities;
using System.Net;
using Moq;

namespace SlackNotification.Tests;

public class SlackNotificationTest
{
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
}
