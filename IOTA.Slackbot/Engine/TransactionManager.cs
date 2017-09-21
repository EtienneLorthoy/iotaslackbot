using IOTA.Slackbot.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Engine
{
    public interface ITransactionManager
    {
        void TmpDeposite(string userId, decimal iotas);
        void RegisterDepositAddress(string userId, string address);
        ResponseMessage GetWalletInfo(string userId);
        ResponseMessage SendTip(string fromUserId, string toUserId, decimal iotas);
    }

    public class TransactionManager : ITransactionManager
    {
        private readonly IWalletRepository _walletRepository;

        public TransactionManager(IWalletRepository walletRepository)
        {
            this._walletRepository = walletRepository;
        }

        public void RegisterDepositAddress(string userId, string address)
        {
            
        }

        public void TmpDeposite(string userId, decimal iotas)
        {
            this._walletRepository.AddIotas(userId, iotas);
        }

        public ResponseMessage GetWalletInfo(string userId)
        {
            var wallet = this._walletRepository.GetWallet(userId);
            return new ResponseMessage(string.Format("You have {0} iotas in your tip wallet. 0 withdraw pending. 0 deposite pending.", wallet.Balance));
        }

        public ResponseMessage SendTip(string fromUserId, string toUserId, decimal iotas)
        {
            if(fromUserId == toUserId)
            {
                return new ResponseMessage("You just tip yourself. Bravo!");
            }

            var wallet = this._walletRepository.GetWallet(fromUserId);

            if(wallet.Balance < iotas)
            {
                return new ResponseMessage("You don't have enough iotas in your wallet.");
            }

            this._walletRepository.TransferIotas(fromUserId, toUserId, iotas);

            return new ResponseMessage(string.Format("You send {0} iotas to {1}.", iotas, toUserId), true);
        }

    }
}
