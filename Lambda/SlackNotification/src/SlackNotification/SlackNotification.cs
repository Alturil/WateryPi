using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SlackNotification;

public class SlackNotification
{
    private ISlackClient _slackClient;

    public SlackNotification()
    {
        var slackUriEnvVar = Environment.GetEnvironmentVariable("SLACK_URI");
        var slackToken = Environment.GetEnvironmentVariable("SLACK_TOKEN");

        //foreach (var envVar in new[] = [slackUriEnvVar, slackToken])
        //{
        //    if (string.IsNullOrEmpty(envVar))
        //    {
        //        throw new Exception($"No {envVar.} env var set");
        //    }
        //}        
        _slackClient = new SlackClient(new Uri(slackUriEnvVar!), slackToken!);
    }

    public SlackNotification(ISlackClient slackClient)
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
