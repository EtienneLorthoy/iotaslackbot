using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Wallet
{
    public class Wallet
    {
        public Guid Id { get; set; }

        public string SlackId { get; set; }

        public string SlackUsername { get; set; }

        public decimal Balance { get; set; }
    }
}
