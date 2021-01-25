namespace ZdrateButyPaciaka.DB
{
    public interface IDbConfigurationProvider
    {
        string GetDbConnectionString();
        string GetDbName();
    }
}