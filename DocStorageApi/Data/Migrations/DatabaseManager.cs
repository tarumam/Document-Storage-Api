using Dapper;
using Npgsql;

namespace DocStorageApi.Data.Migrations
{
    public class DatabaseManager
    {
        private string _connectionString;
        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Tries to create a database if it doesn't exists.
        /// This is called only on the system startup
        /// </summary>
        /// <param name="dbName">Database name</param>
        /// <returns>connection string if success</returns>
        public string TryCreateDatabase(string dbName)
        {
            var connectionBuilder = new NpgsqlConnectionStringBuilder(_connectionString);
            connectionBuilder.Remove("database");

            using var connection = new NpgsqlConnection(connectionBuilder.ToString());

            var query = "SELECT 1 FROM pg_database WHERE datname = @name";
            var parameters = new { name = dbName };

            try
            {
                var result = connection.QuerySingleOrDefault<int>(query, parameters);

                if (result == 0)
                    connection.Execute($"CREATE DATABASE {dbName}");

                connectionBuilder.Add("database", dbName);

                return connectionBuilder.ToString();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }
    }
}
