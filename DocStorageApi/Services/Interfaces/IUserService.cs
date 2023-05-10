using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<Guid?> CreateUserAsync(CreateUserRequest userRequest);
        Task<bool> UpdateUserAsync(UpdateUserRequest userRequest);
        Task<bool> UpdateTokenIdAsync(Guid userId, string tokenId);
        Task<bool> DisableUserAsync(Guid userId);
        Task<UserResponse> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserResponse>> ListUsersAsync();
    }
}
