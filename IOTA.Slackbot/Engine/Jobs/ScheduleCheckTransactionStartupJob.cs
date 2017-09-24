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

            foreach (var index in indexesToVerify)
            {
                var address = _iotaManager.CreateAddress(index);

                ///// todo verify transaction into this address and credit user.
            }
        }
    }
}
