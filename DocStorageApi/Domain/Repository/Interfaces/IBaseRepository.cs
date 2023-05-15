using DocStorageApi.Data.Commands;

namespace DocStorageApi.Domain.Repository.Interfaces
{
    public interface IBaseRepository<T>
    {
        [Obsolete("This method is deprecated and will be removed in a future release, use ExecuteScalarAsync instead.")]
        Task<CommandResult<TReturn>> ExecuteCommand<TReturn>(IBaseCommand command);

        Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string query, object param = null);

        Task<TReturn> QueryFirstOrDefaultAsync<TReturn>(string query, object param = null);

    }
}
