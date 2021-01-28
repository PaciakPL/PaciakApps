namespace DAL.Configuration
{
    public interface IDbConfigurationProvider
    {
        string GetDbConnectionString(string databaseName);
    }
}