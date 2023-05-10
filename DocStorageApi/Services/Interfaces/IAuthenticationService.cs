using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserCredentialsResult> SignInAsync(AuthRequest authRequest);
    }
}
