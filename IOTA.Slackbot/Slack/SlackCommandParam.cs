using System;
using System.Collections.Generic;
using System.Linq;

namespace IOTA.Slackbot.Slack
{
    public class SlackCommandParam
    {
        public string token { get; set; }

        public string channel_id { get; set; }

        public string channel_name { get; set; }

        public string user_id { get; set; }

        public string user_name { get; set; }

        public string command { get; set; }

        public string text { get; set; }

        public string response_url { get; set; }
    }
}