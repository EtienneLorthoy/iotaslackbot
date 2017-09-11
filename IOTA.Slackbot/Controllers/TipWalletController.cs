using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IOTA.Slackbot.Slack;
using Microsoft.Extensions.Options;

namespace IOTA.Slackbot.Controllers
{
    [Route("api/tipwallet")]
    public class TipWalletController : Controller
    {
        private readonly ISlackApiClient _slackApiClient;
        private readonly IOptions<IotaBotSettings> _iotaBotSettings;

        public TipWalletController(
            ISlackApiClient slackApiClient,
            IOptions<IotaBotSettings> iotaBotSettings)
        {
            this._slackApiClient = slackApiClient;
            this._iotaBotSettings = iotaBotSettings;
        }

        [HttpGet]
        [Route("info")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("deposite")]
        public async Task<IActionResult> Desposite([FromBody]SlackCommandParam commandParam)
        {
            // check token
            if (commandParam.token != this._iotaBotSettings.Value.SlackBotToken)
            {
                return this.BadRequest("invalid slack token : " + this._iotaBotSettings.Value.SlackBotToken);
            }

            if (string.IsNullOrEmpty(commandParam.response_url))
            {
                return this.BadRequest("no response url");
            }

            // send iota adress
            await this._slackApiClient.SendMessage(commandParam.response_url, "test 11");

            return this.Ok();
        }

        [HttpPost]
        [Route("withdraw")]
        public void Withdraw([FromBody]string value)
        {
        }

        [HttpPost]
        [Route("sendtip")]
        public void Sendtip([FromBody]string value)
        {
        }
    }
}
