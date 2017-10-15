using System;

namespace IOTA.Slackbot
{
    public class IotaBotSettings
    {
        public string SlackBotToken { get; set; }

        public string IotaNodeUrl { get; set; }

        public int IotaNodePort { get; set; }

        public string IotaWalletSeed { get; set; }
    }
}