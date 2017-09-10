using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IOTA.Slackbot.Slack;

namespace IOTA.Slackbot.Controllers
{
    [Route("api/tipwallet")]
    public class TipWalletController : Controller
    {
        private readonly ISlackApiClient _slackApiClient;

        public TipWalletController(ISlackApiClient slackApiClient)
        {
            this._slackApiClient = slackApiClient;
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
            if (commandParam.token != "TODO")
            {
               return this.BadRequest("invalid slack token");
            }

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
