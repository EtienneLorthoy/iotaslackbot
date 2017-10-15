using FluentScheduler;
using IOTA.Slackbot.Iota.Commands;
using IOTA.Slackbot.Slack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Engine.Jobs
{
    public class CreateIotaAddressJob : IJob
    {
        public string UserId { get; }
        public string ResponseUrl { get; }

        private readonly ISlackApiClient _slackApiClient;
        private readonly GetNextDepositAddressCommand _getNextDepositAddressCommand;

        public CreateIotaAddressJob(string userId, string responseUrl,
            ISlackApiClient slackApiClient,
            GetNextDepositAddressCommand getNextDepositAddressCommand)
        {
            this.UserId = userId;
            this.ResponseUrl = responseUrl;
            _slackApiClient = slackApiClient;
            _getNextDepositAddressCommand = getNextDepositAddressCommand;
        }

        public void Execute()
        {
            var result = this._getNextDepositAddressCommand.Execute(this.UserId);
            var message = $"Please send your iota to this address : {result}";

            //await this._slackApiClient.SendMessage(this.ResponseUrl, message, false);
        }
    }
}