
namespace SlackNotification
{
    public interface IAwsParameterStoreClient
    {
        Task<string> GetValueAsync(string parameter);
    }
}