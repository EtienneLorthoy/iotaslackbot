using System;
using LiteDB;

namespace IOTA.Slackbot.Iota.Repositories
{
    /// <inheritdoc />
    /// <summary>
    /// Repository to act like as a one time usable token provider for each tuple userid/username. 
    /// Each user can have only one token at a time.
    /// </summary>
    public interface IUniqueIndexRepository : IDisposable
    {
        int GetOrSetNextUniqueIndex(string userName, string userId);
    }

    internal class UniqueIndexRepository : IUniqueIndexRepository
    {
        private static string UniqueCollectionPath = @"data/uniqueindexes.db";
        private static string UniqueCollectionName = "uniqueindexes";

        private readonly LiteDatabase _db;
        private readonly LiteCollection<UserUniqueIndex> _collection;

        public UniqueIndexRepository()
        {
            this._db = new LiteDatabase(UniqueCollectionPath);
            _collection = this._db.GetCollection<UserUniqueIndex>(UniqueCollectionName);
        }

        public int GetOrSetNextUniqueIndex(string userName, string userId)
        {
            var uniqueIndex = this._collection.FindOne(u => u.UserId == userId && u.Username == userName);

            if (uniqueIndex == null)
            {
                // We just find the next already used uniqueId and give it to the user
                uniqueIndex = this._collection.FindOne(u => u.Used);

                if (uniqueIndex != null)
                {
                    uniqueIndex.Used = false;
                    uniqueIndex.UserId = userId;
                    uniqueIndex.Username = userName;
                }
                // if all of uniqueId aren't used yet, create a new one
                else
                {
                    var nextUniqueIndex = this._collection.Count();
                    uniqueIndex = new UserUniqueIndex
                    {
                        UniqueIndex = nextUniqueIndex,
                        Used = false,
                        UserId = userId,
                        Username = userName
                    };
                }

                this._collection.EnsureIndex(x => x.UniqueIndex, true);
                this._collection.Update(uniqueIndex)
            }
            // If not null the user didn't used its uniqueId yet, simply resend it.

            return uniqueIndex.UniqueIndex;;
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
