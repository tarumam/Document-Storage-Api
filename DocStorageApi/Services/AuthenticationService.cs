using DocStorageApi.Domain.Repository.Interfaces;
using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using DocStorageApi.Services.Interfaces;
using DocStorageApi.Utils;

namespace DocStorageApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserCredentialsResult> SignInAsync(AuthRequest authInfo)
        {
            var hashedPass = HashPassword.HashIt(authInfo.password);
            var userInfo = await _userRepository.GetUserByCredentials(authInfo.userName, hashedPass);
            return userInfo;
        }
    }
}
