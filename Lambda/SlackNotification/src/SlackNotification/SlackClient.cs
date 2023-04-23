using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SlackNotification;

public class SlackClient
{
    private HttpClient _slackClient;    

    public SlackClient(Uri slackUri)
    {
        _slackClient = new HttpClient
        {
            BaseAddress = slackUri
        };
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
            "application/json"); ;

        using HttpResponseMessage response = await _slackClient.PostAsync("", jsonContent);

        return response;
    }   
}

