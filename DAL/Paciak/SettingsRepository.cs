using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Configuration;
using DAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.Paciak
{
    public class SettingsRepository : ISettingsRepository
    {
        private const string DbName = "forume_apps";
        private const string CollectionName = "musichole_settings";
        private readonly IMongoDatabase db;

        public SettingsRepository(IDbProvider dbProvider)
        {
            db = dbProvider.GetDatabase(DbName);
        }

        public async Task<Option> GetOrDefault(string name, Option @default = default)
        {
            var settingsCollection = GetSettingsCollection();
            using var result = await settingsCollection.FindAsync<Option>(
                Builders<BsonDocument>.Filter.Eq(nameof(Option.Name), name)
            );
            var option = result.FirstOrDefault();

            return option ?? @default;
        }

        public async Task<bool> Upsert(Option option)
        {
            var settingsCollection = GetSettingsCollection();
            var result = await settingsCollection.UpdateOneAsync(
                    Builders<BsonDocument>.Filter.Eq(nameof(Option.Name), option.Name),
                    new BsonDocument("$set", option.ToBsonDocument()),
                    new UpdateOptions() { IsUpsert = true }
                );
            
            return result.IsAcknowledged;
        }

        public async Task<bool> Delete(string name)
        {
            var settingsCollection = GetSettingsCollection();
            var result = await settingsCollection.DeleteOneAsync(Builders<BsonDocument>.Filter.Eq(nameof(Option.Name), name));
            return result.DeletedCount > 0;
        }

        private IMongoCollection<BsonDocument> GetSettingsCollection()
        {
            return db.GetCollection<BsonDocument>(CollectionName);
        }
    }
}