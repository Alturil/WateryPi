using Amazon.Lambda.Core;
using System.Configuration;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SlackNotification;

public class SlackNotification
{
    private SlackClient _slackClient;

    public SlackNotification()
    {
        var slackuri = ConfigurationManager.AppSettings["slackUri"];
        if (slackuri == null)
        {
            throw new ArgumentNullException(nameof(slackuri));
        }

        _slackClient = new SlackClient(new Uri(slackuri!)); ;
    }

    public SlackNotification(SlackClient slackClient)
    {
        _slackClient = slackClient;
    }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> SendSlackNotification(string input, ILambdaContext context)
    {
        context.Logger.Log($"Hello from Lambda! - {input}");
        return await _slackClient.SendTapEventAsync("Alturil");
    }
}
