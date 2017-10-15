using FluentScheduler;

namespace IOTA.Slackbot.Engine.Jobs
{
    public class CheckTransactionsJob : IJob
    {
        private string _addressToVerify;

        public CheckTransactionsJob(string addressToVerify)
        {
            _addressToVerify = addressToVerify;
        }

        public void Execute()
        {
            
        }
    }
}
