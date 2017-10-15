using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Wallet
{
    public class TransactionSummary
    {
        public string Address { get; set; }

        public string Value { get; set; }

        public string Tag { get; set; }

        public string Timestamp { get; set; }
    }
}
