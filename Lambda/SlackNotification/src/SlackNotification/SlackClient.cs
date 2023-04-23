using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SlackNotification;

public class SlackClient : ISlackClient
{
    private readonly HttpClient _slackClient;

    public SlackClient(Uri slackUri, string slackToken)
    {
        _slackClient = new HttpClient
        {
            BaseAddress = slackUri
        };
        _slackClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", slackToken);
    }

    public async Task<HttpResponseMessage> SendTapEventAsync(string user)
    {
        return await SendSlackMessageAsync(":waterline: Tap event", user);
    }

    private async Task<HttpResponseMessage> SendSlackMessageAsync(string title, string user)
    {
        var json = new
        {
            blocks = new object[]
            {
                new
                {
                    type = "header",
                    text = new
                    {
                        type = "plain_text",
                        text = $"{title}",
                        emoji = true
                    }
                },
                new
                {
                    type = "section",
                    fields = new []
                    {
                        new
                        {
                            type = "mrkdwn",
                            text = $"*Time:*\n:clock1: {DateTimeOffset.Now:T}"
                        },
                        new
                        {
                            type = "mrkdwn",
                            text = $"*Triggered by by:*\n :bust_in_silhouette: {user}"
                        }
                    }
                }
            }
        };

        using StringContent jsonContent = new(
            JsonSerializer.Serialize(json),
            Encoding.UTF8,
            "application/json");

        using HttpResponseMessage response = await _slackClient.PostAsync("", jsonContent);

        return response;
    }
}

