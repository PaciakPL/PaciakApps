using MongoDB.Driver;

namespace DAL.Configuration
{
    public interface IDbProvider
    {
        IMongoDatabase GetDatabase(string databaseName);
    }
}