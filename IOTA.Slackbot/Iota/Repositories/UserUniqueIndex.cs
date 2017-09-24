using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Iota.Repositories
{
    internal class UserUniqueIndex
    {
        // Int allow us 2 billions values, hope will never have more pending transaction.
        public int UniqueIndex { get; set; }

        public bool Used { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }
    }
}
