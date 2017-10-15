using System;
using System.Collections.Generic;
using System.Linq;

namespace IOTA.Slackbot.Slack
{
    public class SlackMessageParam
    {
        public SlackMessageParam(string text, bool isPublic)
        {
            this.text = text;
            this.response_type = isPublic ? "in_channel" : "ephemeral";
        }

        public string text { get; }

        public string response_type { get;  }
    }
}