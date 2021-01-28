using System.Collections.Generic;
using MongoDB.Driver;

namespace DAL.Configuration
{
    public class DbProvider : IDbProvider
    {
        private readonly Dictionary<string, MongoClient> dbClients = new();
        private readonly IDbConfigurationProvider configurationProvider;

        public DbProvider(IDbConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public IMongoDatabase GetDatabase(string databaseName)
        {
            if (dbClients.ContainsKey(databaseName))
            {
                return dbClients[databaseName].GetDatabase(databaseName);
            }

            var dbClient = new MongoClient(configurationProvider.GetDbConnectionString(databaseName));

            dbClients[databaseName] = dbClient;

            return dbClient.GetDatabase(databaseName);
        }
    }
}