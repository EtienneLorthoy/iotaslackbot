using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iota.Lib.CSharp.Api;
using Iota.Lib.CSharp.Api.Core;
using Iota.Lib.CSharp.Api.Model;
using Iota.Lib.CSharp.Api.Utils;
using Microsoft.Extensions.Options;

namespace IOTA.Slackbot.Iota
{
    public interface IIotaManager
    {

    }

    public class IotaManager : IIotaManager
    {
        private readonly IOptions<IotaBotSettings> _iotaBotSettings;

        public IotaManager(IOptions<IotaBotSettings> iotaBotSettings)
        {
            this._iotaBotSettings = iotaBotSettings;
        }

        private IotaApi BuildIotaApi()
        {
            return new IotaApi(_iotaBotSettings.Value.IotaNodeUrl, _iotaBotSettings.Value.IotaNodePort);
        }

        public string CreateAddress()
        {
            IotaApi iotaApi = BuildIotaApi();




            return string.Empty;
        }

        public bool SendIotas(string address)
        {
            IotaApi iotaApi = BuildIotaApi();

            if(address.Length == Constants.AddressLengthWithChecksum)
            {
                address = Checksum.RemoveChecksum(address);
            }

            List<Transfer> transfers = new List<Transfer>();
            transfers.Add(new Transfer(address, 0, "JUSTANOTHERTEST", "COTASPAM9999999999999999999"));
            bool[] result = iotaApi.SendTransfer(_iotaBotSettings.Value.IotaWalletSeed, 9, 18, transfers.ToArray(), null, null);

            return result[0];
        }
    }
}
