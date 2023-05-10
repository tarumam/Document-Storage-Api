using Dapper;
using DocStorageApi.Data.DbScripts;

namespace DocStorageApi.Data.Migrations
{
    public class DatabaseObjectsManager
    {
        private readonly DbSession _dbSession;
        private readonly ILogger<DatabaseObjectsManager> _logger;

        public DatabaseObjectsManager(DbSession dbSession, ILogger<DatabaseObjectsManager> logger)
        {
            _dbSession = dbSession;
            _logger = logger;
        }

        public void CreateObjects()
        {
            try
            {
                // TODO: Put folder path on config
                var databaseObjects = DbObjects.LoadScriptsFromFolder(@"Data\DbScripts\Functions");

                if (!databaseObjects.Any()) return;

                foreach (var databaseObject in databaseObjects)
                {
                    // Execute script
                    _logger.LogInformation(@$"Executing: Data\DbScripts\DbObjects\{databaseObject.Key}.sql");
                    _dbSession.Connection.Execute(databaseObject.Value);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
