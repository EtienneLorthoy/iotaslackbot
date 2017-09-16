using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Wallet
{
    public interface IWalletRepository
    {
        Wallet CreateWallet(string userId, string userName);
        Wallet GetWallet(string userId);
        decimal AddIotas(string userId, decimal iotas);
        decimal RemoveIotas(string userId, decimal iotas);
        void TransferIotas(string fromUserId, string toUserId, decimal iotas);
    }

    public class WalletRepository : IWalletRepository
    {
        private static string WalletCollectionPath = @"data/wallets.db";
        private static string WalletCollectionName = "wallets";

        public Wallet CreateWallet(string userId, string userName)
        {
            using (var db = new LiteDatabase(WalletCollectionPath))
            {
                var col = db.GetCollection<Wallet>(WalletCollectionName);

                var wallet = col.FindOne(w => w.SlackId == userId);

                if (wallet != null)
                {
                    throw new InvalidOperationException("Wallet already exists");
                }

                var newWallet = new Wallet
                {
                    SlackId = userId,
                    SlackUsername = userName,
                    Balance = 0
                };

                col.EnsureIndex(x => x.SlackId, true);
                col.Insert(newWallet);

                return newWallet;
            }
        }

        public Wallet GetWallet(string userId)
        {
            using (var db = new LiteDatabase(WalletCollectionPath))
            {
                var col = db.GetCollection<Wallet>(WalletCollectionName);

                return col.FindOne(w => w.SlackId == userId);
            }
        }

        public decimal AddIotas(string userId, decimal iotas)
        {
            using (var db = new LiteDatabase(WalletCollectionPath))
            {
                var col = db.GetCollection<Wallet>(WalletCollectionName);

                var wallet = col.FindOne(w => w.SlackId == userId);

                if(wallet == null)
                {
                    throw new InvalidOperationException("Wallet does not exist");
                }

                wallet.Balance += iotas;

                col.EnsureIndex(x => x.SlackId, true);
                col.Update(wallet);

                return wallet.Balance;
            }
        }

        public decimal RemoveIotas(string userId, decimal iotas)
        {
            using (var db = new LiteDatabase(WalletCollectionPath))
            {
                var col = db.GetCollection<Wallet>(WalletCollectionName);

                var wallet = col.FindOne(w => w.SlackId == userId);

                if (wallet == null)
                {
                    throw new InvalidOperationException("Wallet does not exist");
                }

                if(wallet.Balance - iotas < 0)
                {
                    throw new InvalidOperationException("Not sufficient funds");
                }

                wallet.Balance -= iotas;

                col.EnsureIndex(x => x.SlackId, true);
                col.Update(wallet);

                return wallet.Balance;
            }
        }

        public void TransferIotas(string fromUserId, string toUserId, decimal iotas)
        {
            using (var db = new LiteDatabase(WalletCollectionPath))
            {
                var col = db.GetCollection<Wallet>(WalletCollectionName);

                var fromWallet = col.FindOne(w => w.SlackId == fromUserId);
                var toWallet = col.FindOne(w => w.SlackId == toUserId);

                if (fromWallet == null || toWallet == null)
                {
                    throw new InvalidOperationException("Wallet does not exist");
                }

                if (fromWallet.Balance - iotas < 0)
                {
                    throw new InvalidOperationException("Not sufficient funds");
                }

                fromWallet.Balance -= iotas;
                toWallet.Balance += iotas;

                col.EnsureIndex(x => x.SlackId, true);
                col.Update(new List<Wallet> { fromWallet, toWallet });
            }
        }
    }
}
