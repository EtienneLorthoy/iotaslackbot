using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace IOTA.Slackbot.Slack
{
    public interface ISlackApiClient
    {
        Task<SlackResponse> SendMessage(string url, string text);
    }

    public class SlackApiClient : ISlackApiClient
    {
        public Task<SlackResponse> SendMessage(string url, string text)
        {
            var httpClient = new HttpClient();
            return httpClient.PostJsonAsync<SlackResponse>(
                url,
                new SlackMessageParam
                {
                    text = text
                });
        }

    }
}