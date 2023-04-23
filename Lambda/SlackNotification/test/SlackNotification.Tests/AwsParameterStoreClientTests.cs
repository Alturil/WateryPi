using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SlackNotification.Tests;

public class AwsParameterStoreClientTests
{
    [Fact]
    public async Task When_ParameterExists_Then_ReturnsValue()
    {
        // Arrange
        var mockResponse = new GetParameterResponse
        {
            HttpStatusCode = HttpStatusCode.OK,
            Parameter = new Parameter
            {
                Value = "Bar"
            }
        };

        var mockClient = GetMockClient(mockResponse);
        var awsParameterStoreClient = new AwsParameterStoreClient(mockClient.Object);

        // Act
        var secretValue = await awsParameterStoreClient.GetValueAsync("Foo");

        // Assert
        Assert.Equal("Bar", secretValue);
    }

    [Fact]
    public async Task When_ParameterCantBeRetrieved_Then_ThrowsException()
    {
        // Arrange
        var mockResponse = new GetParameterResponse
        {
            HttpStatusCode = HttpStatusCode.NotFound,            
        };

        var mockClient = GetMockClient(mockResponse);
        var awsParameterStoreClient = new AwsParameterStoreClient(mockClient.Object);

        // Act and Assert
        await Assert.ThrowsAsync<Exception>(async ()=>
            await awsParameterStoreClient.GetValueAsync("Foo"));
    }

    private static Mock<IAmazonSimpleSystemsManagement> GetMockClient(GetParameterResponse mockResponse)
    {
        var mockClient = new Mock<IAmazonSimpleSystemsManagement>();
        mockClient.Setup(c => c.GetParameterAsync(It.IsAny<GetParameterRequest>(), CancellationToken.None))
            .ReturnsAsync(mockResponse);

        return mockClient;
    }
}
