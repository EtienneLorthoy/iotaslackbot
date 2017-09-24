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
        string CreateAddress(int uniqueIndex);
    }

    internal class IotaManager : IIotaManager
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

        public string CreateAddress(int uniqueIndex)
        {
            IotaApi iotaApi = BuildIotaApi();

            var newAddress = iotaApi.GetNewAddress(_iotaBotSettings.Value.IotaWalletSeed, uniqueIndex, false, 1, false);

            return newAddress[0];
        }

        public void GetInputs()
        {
            IotaApi iotaApi = BuildIotaApi();
            Inputs res = iotaApi.GetInputs(_iotaBotSettings.Value.IotaWalletSeed, 0, 0, 0);

            var totalBalance = res.TotalBalance;
        }

        public void FindTransactionObjects(string address)
        {
            IotaApi iotaApi = BuildIotaApi();

            List<Transaction> ftr = iotaApi.FindTransactionObjects(new string[] { address });
        }

        public void GetAllTransfers()
        {
            IotaApi iotaApi = BuildIotaApi();

            var bundles = iotaApi.GetTransfers(_iotaBotSettings.Value.IotaWalletSeed, 0, 0, false);
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
