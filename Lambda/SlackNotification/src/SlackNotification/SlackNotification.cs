using Amazon.Lambda.Core;
using Amazon.SimpleSystemsManagement;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SlackNotification;

public class SlackNotification
{
    private ISlackClient _slackClient;
    private IAwsParameterStoreClient _ssmClient;

    public SlackNotification()
    {      
        _ssmClient = new AwsParameterStoreClient(new AmazonSimpleSystemsManagementClient());
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
        if (_slackClient == null)
        {
            var slackUri = await _ssmClient.GetValueAsync("slack_uri");
            var slackToken = await _ssmClient.GetValueAsync("slack_token");

            _slackClient = new SlackClient(new Uri(slackUri), slackToken);
        }    

        context.Logger.Log($"Sending Slack message - {input}");
        
        return await _slackClient.SendTapEventAsync("Alturil");
    }
}
