using DocStorageApi.Data.Queries;
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
            var salt = await _userRepository.GetSaltByUserName(new GetSaltByUsername(authInfo.userName));
            var hashedPass = HashPassword.HashIt(authInfo.password + salt);
            var userInfo = await _userRepository.GetUserByCredentials(authInfo.userName, hashedPass);
            return userInfo;
        }
    }
}
