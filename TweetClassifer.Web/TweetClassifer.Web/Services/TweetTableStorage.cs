using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Authentication;
using TweetClassifer.Web.Models;
using TweetClassifer.Shared.Models;
using MongoDB.Bson;

namespace TweetClassifer.Web.Services
{
    public class TweetTableStorage
    {
        readonly IMongoCollection<TweetEntry> collection;

        #region Constructors
        public TweetTableStorage(IConfiguration configuration)
        {
            var section = configuration.GetSection("mongo");
            var settings = GetSettings(section);
            var client = new MongoClient(settings);
            var db = client.GetDatabase(section["database"]);
            collection = db.GetCollection<TweetEntry>(section["collection"]);
        }
        #endregion

        #region public methods
        public async Task<int> TotalCount()
        {
            var cursor = await collection.FindAsync((entry) => true);
            return cursor.ToList().Count();
        }

        public async Task InsertRange(IEnumerable<TweetVM> tweets)
        {
            var count = await TotalCount();
            if (count + tweets.Count() > 10_000)
                tweets = tweets.Take(10_000 - count);
            var entries = tweets.Select(tweet => new TweetEntry { Tweet = tweet, IsClassified = false });
            await collection.InsertManyAsync(entries);
        }

        public async Task<TweetEntry> GetTweet(string Id)
        {
            var objId = ObjectId.Parse(Id);
            var result = await collection.FindAsync((entry) => entry.ID == objId);
            return result.First();
        }

        public async Task<List<TweetEntry>> GetUnClassified()
        {
            var result = await collection.FindAsync((entry) => !entry.IsClassified);
            return result.ToList();
        }

        public async Task<List<TweetEntry>> GetClassified()
        {
            var result = await collection.FindAsync((entry) => entry.IsClassified);
            return result.ToList();
        }

        public async Task<(int males, int female, int other)> GetStats()
        {
            var cursor = (await collection.FindAsync((entry) => entry.IsClassified)).ToList();
            var males = cursor.Where(entry => entry.Tweet.Gender == Gender.Male).Count();
            var females = cursor.Where(entry => entry.Tweet.Gender == Gender.Female).Count();
            var other = cursor.Where(entry => entry.Tweet.Gender == Gender.Other).Count();
            return (males, females, other);
        }

        public async Task Remove(string id)
        {
            var objId = ObjectId.Parse(id);
            await collection.DeleteOneAsync((entry) => entry.ID == objId);
        }

        public Task UpdateTweet(TweetEntry entry)
            => collection.FindOneAndReplaceAsync(
                (original) => original.ID == entry.ID,
                entry);
        #endregion

        #region private methods
        private MongoClientSettings GetSettings(IConfigurationSection section)
        {
            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(section["host"], int.Parse(section["port"])),
                UseSsl = true,
                SslSettings = new SslSettings()
            };
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
            var identity = new MongoInternalIdentity(section["database"], section["username"]);
            var evidence = new PasswordEvidence(section["password"]);
            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);
            return settings;
        }
        #endregion
    }
}
