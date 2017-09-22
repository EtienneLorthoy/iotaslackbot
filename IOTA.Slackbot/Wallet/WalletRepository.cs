using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Wallet
{
    public interface IWalletRepository
    {
        Wallet CreateWallet(string userId);
        Wallet GetWallet(string userId);
        decimal AddIotas(string userId, decimal iotas);
        decimal RemoveIotas(string userId, decimal iotas);
        void TransferIotas(string fromUserId, string toUserId, decimal iotas);
    }

    public class WalletRepository : IWalletRepository, IDisposable
    {
        private static string WalletCollectionPath = @"data/wallets.db";
        private static string WalletCollectionName = "wallets";

        private readonly LiteDatabase _db;
        private readonly LiteCollection<Wallet> _collection;

        public WalletRepository()
        {
            this._db = new LiteDatabase(WalletCollectionPath);
            this._collection = this._db.GetCollection<Wallet>(WalletCollectionName);
        }

        public Wallet CreateWallet(string userId)
        {
            var wallet = this._collection.FindOne(w => w.SlackId == userId);

            if (wallet != null)
            {
                throw new InvalidOperationException("Wallet already exists");
            }

            var newWallet = new Wallet
            {
                SlackId = userId,
                Balance = 0
            };

            this._collection.EnsureIndex(x => x.SlackId, true);
            this._collection.Insert(newWallet);

            return newWallet;
        }

        public Wallet GetWallet(string userId)
        {
            return this.GetOrCreateWallet(userId);
        }

        public decimal AddIotas(string userId, decimal iotas)
        {
            var wallet = this.GetOrCreateWallet(userId);

            wallet.Balance += iotas;

            this._collection.EnsureIndex(x => x.SlackId, true);
            this._collection.Update(wallet);

            return wallet.Balance;
        }

        public decimal RemoveIotas(string userId, decimal iotas)
        {
            var wallet = this.GetOrCreateWallet(userId);

            if (wallet.Balance - iotas < 0)
            {
                throw new InvalidOperationException("Not sufficient funds");
            }

            wallet.Balance -= iotas;

            this._collection.EnsureIndex(x => x.SlackId, true);
            this._collection.Update(wallet);

            return wallet.Balance;
        }

        public void TransferIotas(string fromUserId, string toUserId, decimal iotas)
        {
            var fromWallet = this.GetOrCreateWallet(fromUserId);
            var toWallet = this.GetOrCreateWallet(toUserId);

            if (fromWallet.Balance - iotas < 0)
            {
                throw new InvalidOperationException("Not sufficient funds");
            }

            fromWallet.Balance -= iotas;
            toWallet.Balance += iotas;

            this._collection.EnsureIndex(x => x.SlackId, true);
            this._collection.Update(new List<Wallet> { fromWallet, toWallet });
        }

        public void Dispose()
        {
            this._db?.Dispose();
        }

        private Wallet GetOrCreateWallet(string userId)
        {
            return this._collection.FindOne(w =>(w.SlackId == userId)) ?? this.CreateWallet(userId);
        }
    }
}
