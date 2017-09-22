using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IOTA.Slackbot.Slack;
using Microsoft.Extensions.Options;
using IOTA.Slackbot.Engine;
using IOTA.Slackbot.Iota;

namespace IOTA.Slackbot.Controllers
{
    [Route("api/tipwallet")]
    public class TipWalletController : Controller
    {
        private readonly ISlackApiClient _slackApiClient;
        private readonly IOptions<IotaBotSettings> _iotaBotSettings;
        private readonly ITransactionManager _transactionManager;
        private readonly IIotaManager _iotaManager;

        public TipWalletController(
            ISlackApiClient slackApiClient,
            IOptions<IotaBotSettings> iotaBotSettings,
            ITransactionManager transactionManager,
            IIotaManager iotaManager)
        {
            this._slackApiClient = slackApiClient;
            this._iotaBotSettings = iotaBotSettings;
            this._transactionManager = transactionManager;
            this._iotaManager = iotaManager;
        }

        [HttpPost]
        [Route("info")]
        public async Task<IActionResult> Info([FromForm]SlackCommandParam commandParam)
        {
            if (commandParam.token != this._iotaBotSettings.Value.SlackBotToken)
            {
                return this.BadRequest("invalid slack token");
            }

            var message = this._transactionManager.GetWalletInfo(commandParam.SlackUserIdentity);

#if DEBUG
            return this.Ok(message);
#endif

            await this._slackApiClient.SendMessage(commandParam.response_url, message.Text, message.IsPublic);
            return this.Ok();
        }

        [HttpPost]
        [Route("deposite")]
        public async Task<IActionResult> Desposite([FromForm]SlackCommandParam commandParam)
        {
            if (commandParam.token != this._iotaBotSettings.Value.SlackBotToken)
            {
                return this.BadRequest("invalid slack token");
            }

            //var address = this._iotaManager.CreateAddress();

            // Move this to job
            this._transactionManager.TmpDeposite(commandParam.SlackUserIdentity, 500);

            var message = $"Please send your iota to this address : 1273487263827";

#if DEBUG
            return this.Ok(message);
#endif

            await this._slackApiClient.SendMessage(commandParam.response_url, message, false);
            return this.Ok();
        }

        [HttpPost]
        [Route("withdraw")]
        public async Task<IActionResult> Withdraw([FromForm]SlackCommandParam commandParam)
        {
            if (commandParam.token != this._iotaBotSettings.Value.SlackBotToken)
            {
                return this.BadRequest("invalid slack token");
            }

            var message = "We will send your 100 iotas to the address 123456789 shortly.";
#if DEBUG
            return this.Ok(message);
#endif

            await this._slackApiClient.SendMessage(commandParam.response_url, message, false);
            return this.Ok();
        }

        [HttpPost]
        [Route("sendtip")]
        public async Task<IActionResult> Sendtip([FromForm]SlackCommandParam commandParam)
        {
            if (commandParam.token != this._iotaBotSettings.Value.SlackBotToken)
            {
                return this.BadRequest("invalid slack token");
            }

            var matches = Regex.Matches(commandParam.text, @"(\w+)");
            var toUserId = commandParam.channel_id + "_" + matches[0].Value;

            var message = this._transactionManager.SendTip(
                commandParam.SlackUserIdentity,
                toUserId,
                10);

#if DEBUG
            return this.Ok(message);
#endif

            await this._slackApiClient.SendMessage(commandParam.response_url, message.Text, message.IsPublic);
            return this.Ok();
        }
    }
}
