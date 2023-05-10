using DocStorageApi.Data.Commands;
using DocStorageApi.Data.Queries;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<CommandResult<Guid?>> InsertUserAsync(InsertUserCommand command);

        Task<CommandResult<int>> UpdateUserAsync(UpdateUserCommand command);

        Task<CommandResult<int>> DisableUserAsync(DisableUserCommand command);

        Task<CommandResult<int>> UpdateTokenId(UpdateUserTokenIdCommand command);

        Task<UserCredentialsResult> GetUserByCredentials(string username, string password);

        Task<UserResponse> GetUserByIdAsync(GetUserByIdQuery query);

        Task<IEnumerable<UserResponse>> ListUsersAsync(ListAllUsersQuery query);
    }
}
