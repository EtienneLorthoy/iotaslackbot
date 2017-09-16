using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IOTA.Slackbot.Slack;
using Microsoft.Extensions.Options;
using IOTA.Slackbot.Engine;

namespace IOTA.Slackbot.Controllers
{
    [Route("api/tipwallet")]
    public class TipWalletController : Controller
    {
        private readonly ISlackApiClient _slackApiClient;
        private readonly IOptions<IotaBotSettings> _iotaBotSettings;
        private readonly ITransactionManager _transactionManager;

        public TipWalletController(
            ISlackApiClient slackApiClient,
            IOptions<IotaBotSettings> iotaBotSettings,
            ITransactionManager transactionManager)
        {
            this._slackApiClient = slackApiClient;
            this._iotaBotSettings = iotaBotSettings;
            this._transactionManager = transactionManager;
        }

        [HttpPost]
        [Route("info")]
        public async Task<IActionResult> Info([FromForm]SlackCommandParam commandParam)
        {
            if (commandParam.token != this._iotaBotSettings.Value.SlackBotToken)
            {
                return this.BadRequest("invalid slack token");
            }

            var message = this._transactionManager.GetWalletInfo(
                commandParam.user_id,
                commandParam.user_name);

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

            this._transactionManager.TmpDeposite(commandParam.user_id, commandParam.user_name, 500);

            // send iota address
            await this._slackApiClient.SendMessage(commandParam.response_url, "Please send your iota to this address : 1234456789", false);
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

            await this._slackApiClient.SendMessage(commandParam.response_url, "We will send your 100 iotas to the address 123456789 shortly.", false);
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

            var toUserId = "todo";
            var toUserName = "todo";

            var message = this._transactionManager.SendTip(
                commandParam.user_id,
                commandParam.user_name,
                toUserId,
                toUserName,
                10);

            await this._slackApiClient.SendMessage(commandParam.response_url, message.Text, message.IsPublic);
            return this.Ok();
        }
    }
}
