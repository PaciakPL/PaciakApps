using MongoDB.Driver;

namespace ZdrateButyPaciaka.DB
{
    public interface IDbProvider
    {
        IMongoDatabase GetDatabase();
    }
}