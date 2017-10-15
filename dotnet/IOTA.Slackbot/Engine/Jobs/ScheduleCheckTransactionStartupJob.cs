using FluentScheduler;
using IOTA.Slackbot.Iota;
using IOTA.Slackbot.Iota.Repositories;

namespace IOTA.Slackbot.Engine.Jobs
{
    public class ScheduleCheckTransactionStartupJob : IJob
    {
        private readonly IUniqueIndexRepository _uniqueIndexRepository;
        private readonly IIotaManager _iotaManager;

        public ScheduleCheckTransactionStartupJob(IUniqueIndexRepository uniqueIndexRepository,
            IIotaManager iotaManager)
        {
            _uniqueIndexRepository = uniqueIndexRepository;
            _iotaManager = iotaManager;
        }

        public void Execute()
        {
            var indexesToVerify = _uniqueIndexRepository.GetUnusedUniqueIndexes();
            var delay = 0;

            foreach (var index in indexesToVerify)
            {
                var address = _iotaManager.CreateAddress(index);
                
                JobManager.AddJob(new CheckTransactionsJob(address), s => s.ToRunOnceIn(++delay).Seconds());
            }
        }
    }
}
