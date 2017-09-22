using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Slack
{
    public class FakeSlackApiClient : ISlackApiClient
    {
        public Task<SlackResponse> SendMessage(string url, string text, bool isPublic)
        {
            return null;
        }
    }
}
