using DocStorageApi.Data.Commands;
using DocStorageApi.Data.Queries;
using DocStorageApi.Domain.Repository.Interfaces;
using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using DocStorageApi.Services.Interfaces;
using DocStorageApi.Utils;

namespace DocStorageApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid?> CreateUserAsync(CreateUserRequest userRequest)
        {
            string salt = Guid.NewGuid().ToString();
            string hashedPass = HashPassword.HashIt(userRequest.Password + salt);
            var result = await _userRepository.InsertUserAsync(new InsertUserCommand(userRequest.Username, hashedPass, userRequest.Role, salt, true));
            return result.Data;
        }

        public async Task<bool> UpdateUserAsync(UpdateUserRequest request)
        {
            string salt = Guid.NewGuid().ToString();
            string hashedPass = HashPassword.HashIt(request.Password + salt);
            var result = await _userRepository.UpdateUserAsync(new UpdateUserCommand(request.Id, request.Username, hashedPass, request.Role, salt, request.Status));
            return result.Executed;
        }

        public async Task<bool> DisableUserAsync(Guid userId)
        {
            var result = await _userRepository.DisableUserAsync(new DisableUserCommand(userId));
            return result.Executed;
        }

        public async Task<bool> UpdateTokenIdAsync(Guid userId, string tokenId)
        {
            if (string.IsNullOrEmpty(tokenId)) return false;

            var result = await _userRepository.UpdateTokenId(new UpdateUserTokenIdCommand(userId, tokenId));
            return result.Executed;
        }

        public async Task<UserResponse> GetUserByIdAsync(Guid userId)
        {
            return await _userRepository.GetUserByIdAsync(new GetUserByIdQuery(userId));
        }

        public async Task<IEnumerable<UserResponse>> ListUsersAsync()
        {
            return await _userRepository.ListUsersAsync(new ListAllUsersQuery());
        }
    }
}