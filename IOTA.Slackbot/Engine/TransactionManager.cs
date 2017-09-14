using IOTA.Slackbot.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Engine
{
    public interface ITransactionManager
    {
        ResponseMessage GetWalletInfo(string userId, string userName);
        ResponseMessage SendTip(string fromUserId, string fromUserName, string toUserId, string toUserName, decimal iotas);
    }

    public class TransactionManager : ITransactionManager
    {
        private readonly IWalletRepository _walletRepository;

        public TransactionManager(IWalletRepository walletRepository)
        {
            this._walletRepository = walletRepository;
        }

        public ResponseMessage GetWalletInfo(string userId, string userName)
        {
            var wallet = this._walletRepository.GetWallet(userId);

            if (wallet == null)
            {
                wallet = this._walletRepository.CreateWallet(userId, userName);
            }

            return new ResponseMessage(string.Format("You have {0} iotas in your tip wallet", wallet.Balance));
        }

        public ResponseMessage SendTip(string fromUserId, string fromUserName, string toUserId, string toUserName, decimal iotas)
        {
            var wallet = this._walletRepository.GetWallet(fromUserId);
            if(wallet == null)
            {
                wallet = this._walletRepository.CreateWallet(fromUserId, fromUserName);
            }

            this._walletRepository.AddIotas(fromUserId, 200);

            return new ResponseMessage("You send 100 iotas to etienne", true);
        }
    }
}
