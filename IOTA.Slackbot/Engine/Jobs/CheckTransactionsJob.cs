using FluentScheduler;
using IOTA.Slackbot.Iota;

namespace IOTA.Slackbot.Engine.Jobs
{
    public class CheckTransactionsJob : IJob
    {
        private readonly IIotaManager _iotaManager;

        private string _addressToVerify;

        public CheckTransactionsJob(string addressToVerify, IIotaManager iotaManager)
        {
            _addressToVerify = addressToVerify;
            _iotaManager = iotaManager;
        }

        public void Execute()
        {

            //var this._iotaManager.
        }
    }
}
