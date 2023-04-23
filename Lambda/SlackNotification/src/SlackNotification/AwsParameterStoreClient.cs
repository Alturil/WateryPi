using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace SlackNotification;

public class AwsParameterStoreClient : IAwsParameterStoreClient
{
    private static IAmazonSimpleSystemsManagement _ssmClient;

    public AwsParameterStoreClient(IAmazonSimpleSystemsManagement ssmClient)
    {
        _ssmClient = ssmClient ?? throw new ArgumentNullException(nameof(ssmClient));
    }

    public async Task<string> GetValueAsync(string parameter)
    {
        var response = await _ssmClient.GetParameterAsync(new GetParameterRequest
        {
            Name = parameter,
            WithDecryption = true
        });

        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception($"Unable to retrieve parameter {parameter} - HTTP {response.HttpStatusCode}");
        }

        return response.Parameter.Value;
    }

}
