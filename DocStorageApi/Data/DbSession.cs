using DocStorageApi.Configuration;
using Npgsql;
using System.Data;

namespace DocStorageApi.Data
{
    public sealed class DbSession : IDisposable
    {
        // Manages connection
        public IDbConnection Connection { get; set; }

        // Manages transactions
        public IDbTransaction Transaction { get; set; }

        public readonly string ConnectionString;

        public DbSession(string connectionString)
        {
            ConnectionString = connectionString;
            Connection = CreateConnection(connectionString);
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();

        private IDbConnection CreateConnection(string connectionString) => new NpgsqlConnection(connectionString);
    }
}