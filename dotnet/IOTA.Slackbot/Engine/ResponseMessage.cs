using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Engine
{
    public class ResponseMessage
    {
        public ResponseMessage(string text, bool isPublic = false)
        {
            this.Text = text;
            this.IsPublic = isPublic;
        }

        public string Text { get; set; }

        public bool IsPublic { get; set; }
    }
}
