using System;
using System.Collections.Generic;
using System.Text;
using IOTA.Slackbot.Engine;
using IOTA.Slackbot.Iota;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IOTA.Slackbot.Tests
{
    [TestClass]
    public class IotaManagerTests
    {
        private IIotaManager _iotaManager;

        [TestInitialize]
        public void Initialize()
        {
            IOptions<IotaBotSettings> options = Options.Create(new IotaBotSettings
            {
                IotaNodePort = 14265,
                IotaNodeUrl = "node.iotawallet.info",
                IotaWalletSeed = "IOTASLACKBOTIOTASLACKBOTIOTASLACKBOTIOTASLACKBOTIOTASLACKBOTIOTASLACKBOTIOTASLA",
                SlackBotToken = string.Empty
            });

            this._iotaManager = new IotaManager(options);
        }

        [TestMethod]
        public void CreateAddress_returnAddress()
        {
            var address = _iotaManager.CreateAddress();

            Assert.IsNotNull(address);
        }
    }
}
