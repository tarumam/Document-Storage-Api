using DocStorageApi.Services.Interfaces;
using System.Security.Authentication;
using System.Security.Claims;

namespace DocStorageApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid GetUserId()
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid result))
            {
                throw new InvalidCredentialException("Can't obtain the current user Id");
            }
            return result;
        }

        public bool IsAuthenticated()
        {
            return _contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        }

    }
}
