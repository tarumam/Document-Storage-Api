using DocStorageApi.Configuration;
using Npgsql;
using System.Data;

namespace DocStorageApi.Data
{
    public sealed class DbSession : IDisposable
    {
        // Manages connection
        public NpgsqlConnection Connection { get; set; }

        // Manages transactions
        public NpgsqlTransaction Transaction { get; set; }

        public readonly string ConnectionString;

        public DbSession(string connectionString)
        {
            ConnectionString = connectionString;
            Connection = CreateConnection(connectionString);
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();

        private NpgsqlConnection CreateConnection(string connectionString) => new NpgsqlConnection(connectionString);
    }
}