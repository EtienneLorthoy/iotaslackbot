using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTA.Slackbot.Iota.Repositories;

namespace IOTA.Slackbot.Iota.Commands
{
    public class GetNextDepositAddressCommand
    {
        private readonly IUniqueIndexRepository _uniqueIndexRepository;
        private readonly IIotaManager _iotaManager;

        public GetNextDepositAddressCommand(IUniqueIndexRepository uniqueIndexRepository,
            IIotaManager iotaManager)
        {
            _uniqueIndexRepository = uniqueIndexRepository;
            _iotaManager = iotaManager;
        }

        public string Execute(string userId, string username)
        {
            var nextIndex = this._uniqueIndexRepository.GetOrSetNextUniqueIndex(username, userId);
            var address = this._iotaManager.CreateAddress(nextIndex);

            return address;
        }
    }
}
