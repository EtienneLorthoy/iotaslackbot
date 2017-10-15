using System;
using System.Collections.Generic;
using System.Linq;
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
        int GetOrSetNextUniqueIndex(string userId);
        IEnumerable<int> GetUnusedUniqueIndexes();
        bool UserHasAddressAssigned(string userId);
    }

    internal class UniqueIndexRepository : IUniqueIndexRepository
    {
        private static string UniqueCollectionPath = @"./data/uniqueindexes.db";
        private static string UniqueCollectionName = "uniqueindexes";

        private readonly LiteDatabase _db;
        private readonly LiteCollection<UserUniqueIndex> _collection;

        public UniqueIndexRepository()
        {
            this._db = new LiteDatabase(UniqueCollectionPath);
            _collection = this._db.GetCollection<UserUniqueIndex>(UniqueCollectionName);
        }

        public int GetOrSetNextUniqueIndex(string userId)
        {
            var uniqueIndex = this._collection.FindOne(u =>!u.Used && u.UserId == userId);

            if (uniqueIndex == null)
            {
                // We just find the next already used uniqueId and give it to the user
                uniqueIndex = this._collection.FindOne(u => u.Used);

                if (uniqueIndex != null)
                {
                    uniqueIndex.Used = false;
                    uniqueIndex.UserId = userId;

                    this._collection.Update(uniqueIndex);
                }
                // if all of uniqueId aren't used yet, create a new one
                else
                {
                    var nextUniqueIndex = this._collection.Count();
                    uniqueIndex = new UserUniqueIndex
                    {
                        Id = nextUniqueIndex,
                        Used = false,
                        UserId = userId
                    };
                    this._collection.Insert(uniqueIndex);
                }

                this._collection.EnsureIndex(x => x.Id, true);
            }
            // If not null the user didn't used its uniqueId yet, simply resend it.

            return uniqueIndex.Id;;
        }

        public IEnumerable<int> GetUnusedUniqueIndexes()
        {
            var document = this._collection.Find(u => u.Used);

            if (document == null && document.GetEnumerator() == null)
            {
                return new List<int>();
            }

            var result = document
                .Select(u => u.Id)
                .ToList();

            return result;
        }

        public bool UserHasAddressAssigned(string userId)
        {
            var uniqueIndex = this._collection.Count(u => !u.Used && u.UserId == userId);

            return uniqueIndex > 0;
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
