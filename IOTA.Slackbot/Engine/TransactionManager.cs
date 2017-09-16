using IOTA.Slackbot.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Engine
{
    public interface ITransactionManager
    {
        void TmpDeposite(string userId, string userName, decimal iotas);
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

        public void TmpDeposite(string userId, string userName, decimal iotas)
        {
            var wallet = this._walletRepository.GetWallet(userId);

            if (wallet == null)
            {
                wallet = this._walletRepository.CreateWallet(userId, userName);
            }

            this._walletRepository.AddIotas(userId, iotas);
        }

        public ResponseMessage GetWalletInfo(string userId, string userName)
        {
            var wallet = this._walletRepository.GetWallet(userId);

            if (wallet == null)
            {
                wallet = this._walletRepository.CreateWallet(userId, userName);
            }

            return new ResponseMessage(string.Format("You have {0} iotas in your tip wallet. 0 withdraw pending. 0 deposite pending.", wallet.Balance));
        }

        public ResponseMessage SendTip(string fromUserId, string fromUserName, string toUserId, string toUserName, decimal iotas)
        {
            if(fromUserId == toUserId)
            {
                return new ResponseMessage("You just tip yourself. Bravo!");
            }

            var wallet = this._walletRepository.GetWallet(fromUserId);
            if(wallet == null)
            {
                wallet = this._walletRepository.CreateWallet(fromUserId, fromUserName);
            }

            if(wallet.Balance < iotas)
            {
                return new ResponseMessage("You don't have enough iotas in your wallet.");
            }

            var toWallet = this._walletRepository.GetWallet(toUserId);
            if (toWallet == null)
            {
                toWallet = this._walletRepository.CreateWallet(toUserId, toUserName);
            }

            this._walletRepository.TransferIotas(fromUserId, toUserId, iotas);

            return new ResponseMessage(string.Format("You send {0} iotas to {1}.", iotas, toUserName), true);
        }
    }
}
