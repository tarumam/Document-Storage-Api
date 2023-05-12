using DocStorageApi.Data;
using DocStorageApi.Data.Commands;
using DocStorageApi.Data.Queries;
using DocStorageApi.Domain.Repository.Interfaces;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository
{
    public class UserRepository : BaseRepository<UserRepository>, IUserRepository
    {
        public UserRepository(DbSession session, ILogger<UserRepository> logger) : base(session, logger)
        {
        }

        public async Task<CommandResult<Guid?>> InsertUserAsync(InsertUserCommand command)
        {
            return await ExecuteCommand<Guid?>(command);
        }

        public async Task<CommandResult<int>> UpdateUserAsync(UpdateUserCommand command)
        {
            return await ExecuteCommand<int>(command);
        }

        public async Task<CommandResult<int>> DisableUserAsync(DisableUserCommand command)
        {
            return await ExecuteCommand<int>(command);
        }

        public async Task<CommandResult<int>> UpdateTokenId(UpdateUserTokenIdCommand command)
        {
            return await ExecuteCommand<int>(command);
        }

        public async Task<UserCredentialsResult> GetUserByCredentials(string username, string password)
        {
            var query = new GetUserCredentialsQuery(username, password);
            return await QueryFirstOrDefaultAsync<UserCredentialsResult>(query.Script, query.Param);
        }

        public async Task<UserResponse> GetUserByIdAsync(GetUserByIdQuery query)
        {
            return await QueryFirstOrDefaultAsync<UserResponse>(query.Script, query.Param);
        }

        public async Task<IEnumerable<UserResponse>> ListUsersAsync(ListAllUsersQuery query)
        {
            return await QueryAsync<UserResponse>(query.Script);
        }

        public async Task<string> GetSaltByUserName(GetSaltByUsername query)
        {
            return await QueryFirstOrDefaultAsync<string>(query.Script, query.Param);
        }

    }
}