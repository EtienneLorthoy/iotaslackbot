using System;
using System.Collections.Generic;
using System.Linq;

namespace IOTA.Slackbot.Slack
{
    public class SlackMessageParam
    {
        public SlackMessageParam()
        {
            // ephemeral => only for user
            this.response_type = "ephemeral";
        }

        public string text { get; set; }

        public string response_type { get; set; }
    }
}