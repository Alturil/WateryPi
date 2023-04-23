using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SlackNotification;

public class SlackNotification
{
    private SlackClient _slackClient;

    public SlackNotification()
    {
        var slackUriEnvVar = Environment.GetEnvironmentVariable("SLACK_URI");
        if (string.IsNullOrEmpty(slackUriEnvVar))
        {
            throw new Exception("No SLACK_URI env var set");
        }
        _slackClient = new SlackClient(new Uri(slackUriEnvVar));
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
