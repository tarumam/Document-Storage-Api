using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository.Interfaces
{
    public interface IUserAccessGroupRepository
    {
        Task<IEnumerable<UserAccessGroupsResponse>> GetUserAccessGroups(Guid? userId);

    }
}
