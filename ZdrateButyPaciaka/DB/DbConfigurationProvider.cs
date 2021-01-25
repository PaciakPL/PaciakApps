using System.Configuration;

namespace ZdrateButyPaciaka.DB
{
    public class DbConfigurationProvider : IDbConfigurationProvider
    {
        private readonly string _dbName;
        private readonly string _connectionString;

        public DbConfigurationProvider()
        {
            _dbName = ConfigurationManager.AppSettings["dbName"];
            _connectionString = $"{ConfigurationManager.ConnectionStrings[_dbName]}/{_dbName}";
        }
        public string GetDbConnectionString()
        {
            return _connectionString;
        }

        public string GetDbName()
        {
            return _dbName;
        }
    }
}