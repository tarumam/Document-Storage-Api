using Dapper;
using DocStorageApi.Data;
using DocStorageApi.Data.Commands;
using DocStorageApi.Domain.Repository.Interfaces;
using Npgsql;

namespace DocStorageApi.Domain.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DbSession _session;
        protected ILogger<T> _logger;

        public BaseRepository(DbSession session, ILogger<T> logger)
        {
            _session = session;
            _logger = logger;
        }


        public async Task<CommandResult<TReturn>> ExecuteCommand<TReturn>(IBaseCommand command)
        {
            try
            {
                if (!command.IsValid())
                {
                    return new CommandResult<TReturn>(command.ValidationResults);
                }

                var result = await _session.Connection.ExecuteScalarAsync<TReturn>(command.Script, command.Param, _session.Transaction);
                return new CommandResult<TReturn>(result);
            }
            catch (PostgresException ex)
            {
                _logger.LogError("Error on {query} with params {param}: {Message}", command.Script, command.Param, ex.Message);
                return new CommandResult<TReturn>(ex.Message, ex.SqlState);
            }
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string query, object param = null)
        {
            try
            {
                return await _session.Connection.QueryAsync<TReturn>(query, param, _session.Transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on {query} with params {param}: {Message}", query, param, ex.Message);
                throw;
            }
        }

        public async Task<TReturn> QueryFirstOrDefaultAsync<TReturn>(string query, object param = null)
        {
            try
            {
                return await _session.Connection.QueryFirstOrDefaultAsync<TReturn>(query, param, _session.Transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on {query} with params {param}: {Message}", query, param, ex.Message);
                throw;
            }
        }

    }
}
