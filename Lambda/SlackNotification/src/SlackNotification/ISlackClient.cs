
namespace SlackNotification;

public interface ISlackClient
{
    Task<HttpResponseMessage> SendTapEventAsync(string user);
}
