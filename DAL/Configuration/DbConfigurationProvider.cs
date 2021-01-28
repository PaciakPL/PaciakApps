using System;
using System.Configuration;

namespace DAL.Configuration
{
    public class DbConfigurationProvider : IDbConfigurationProvider
    {
        public string GetDbConnectionString(string databaseName)
        {
            if (ConfigurationManager.ConnectionStrings[databaseName] == null)
            {
                throw new ArgumentException($"Connection string for {databaseName} does not exist");
            }
            
            return $"{ConfigurationManager.ConnectionStrings[databaseName]}/{databaseName}";
        }
    }
}