using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Services.Interfaces
{
    public interface IAccessControlService
    {
        Task<IEnumerable<AccessGroupResponse>> GetAllAccessGroupsAsync();
        Task<IEnumerable<DocumentPermissionsWithUserResponse>> ListDocumentPermissionsWithUsers(Guid? userId);
        Task<IEnumerable<UserAccessGroupsResponse>> GetUserAccessGroups(Guid? userId);
        Task<bool> AddAccessGroupAsync(string name);
        Task<bool> UpdateAccessGroupAsync(Guid accessGroupId, string name, bool status);
        Task<bool> AssignUserToAccessGroupsAsync(AssignUserToAccessGroupRequest userGroup);
        Task<bool> GrantPermissionForUserAsync(AssignUserToAccessDocumentRequest userDoc);
        Task<bool> GrantPermissionForGroupAsync(AssignGroupToAccessDocumentRequest groupDoc);
        Task<bool> RemoveUserFromAccessGroupsAsync(RemoveUserFromAccessGroupRequest userGroup);
        Task<bool> RemoveUserAccessFromDocument(RemoveUserAccessToDocumentRequest userDoc);
        Task<bool> RemoveUserAccessGroupFromDocument(RemoveAccessGroupToDocumentRequest userGroup);

    }
}
