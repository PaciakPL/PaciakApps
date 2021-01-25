using MongoDB.Driver;

namespace ZdrateButyPaciaka.DB
{
    public class DbProvider : IDbProvider
    {
        private readonly IDbConfigurationProvider _configurationProvider;
        private MongoClient _dbClient = null;

        public DbProvider(IDbConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public IMongoDatabase GetDatabase()
        {
            if (_dbClient == null)
            {
                _dbClient = new MongoClient(_configurationProvider.GetDbConnectionString());
            }

            return _dbClient.GetDatabase(_configurationProvider.GetDbName());
        }
    }
}