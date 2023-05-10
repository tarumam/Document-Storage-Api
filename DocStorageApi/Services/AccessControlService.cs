using DocStorageApi.Data.Commands;
using DocStorageApi.Domain.Repository.Interfaces;
using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using DocStorageApi.Services.Interfaces;

namespace DocStorageApi.Services
{
    public class AccessControlService : IAccessControlService
    {
        private readonly IAccessGroupRepository _accessGroupRepository;
        private readonly IDocAccessGroupRepository _docAccessGroupRepository;
        private readonly IDocAccessUserRepository _docAccessUserRepository;
        private readonly IUserAccessGroupRepository _userAccessGroupRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly Guid _currentUserId;

        public AccessControlService(IAccessGroupRepository accessGroupRepository,
            IDocAccessGroupRepository docAccessGroupRepository,
            IDocAccessUserRepository docAccessUserRepository,
            ICurrentUserService currentUserService,
            IUserAccessGroupRepository userAccessGroupRepository,
            IDocumentRepository documentRepository)
        {
            _currentUserId = currentUserService.GetUserId();
            _accessGroupRepository = accessGroupRepository;
            _docAccessGroupRepository = docAccessGroupRepository;
            _docAccessUserRepository = docAccessUserRepository;
            _userAccessGroupRepository = userAccessGroupRepository;
            _documentRepository = documentRepository;
        }

        public async Task<IEnumerable<AccessGroupResponse>> GetAllAccessGroupsAsync()
        {
            return await _accessGroupRepository.GetAllAccessGroupsAsync();
        }

        public async Task<bool> AssignUserToAccessGroupsAsync(AssignUserToAccessGroupRequest userGroup)
        {
            var command = new AssignUserToAccessGroupCommand(userGroup.userId, userGroup.accessGroupId, _currentUserId);
            var result = await _accessGroupRepository.AssignUserToAccessGroupAsync(command);
            return result.Succeeded;
        }

        public async Task<bool> AddAccessGroupAsync(string name)
        {
            var command = new InsertAccessGroup(name);
            var result = await _accessGroupRepository.AddAccessGroupAsync(command);
            return result.Succeeded;
        }

        public async Task<bool> UpdateAccessGroupAsync(Guid accessGroupId, string name, bool status)
        {
            var command = new UpdateAccessGroupCommand(accessGroupId, name, status);
            var result = await _accessGroupRepository.UpdateAccessGroupAsync(command);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromAccessGroupsAsync(RemoveUserFromAccessGroupRequest userGroup)
        {
            var command = new RemoveUserAccessGroupCommand(userGroup.userId, userGroup.groupId);
            var result = await _accessGroupRepository.RemoveUserFromAccessGroupAsync(command);
            return result.Succeeded;
        }

        public async Task<bool> GrantPermissionForUserAsync(AssignUserToAccessDocumentRequest userDoc)
        {
            var command = new AssignUserToDocumentCommand(userDoc.UserId, userDoc.DocumentId, _currentUserId);
            var result = await _docAccessUserRepository.GrantDocPermissionForUserAsync(command);
            return result.Succeeded;
        }

        public async Task<bool> GrantPermissionForGroupAsync(AssignGroupToAccessDocumentRequest groupDoc)
        {
            var command = new AssignGroupToDocumentCommand(groupDoc.AccessGroupId, groupDoc.DocumentId, _currentUserId);
            var result = await _docAccessGroupRepository.GrantDocPermissionForGroupAsync(command);
            return result.Succeeded;
        }

        public async Task<IEnumerable<DocumentPermissionsWithUserResponse>> ListDocumentPermissionsWithUsers(Guid? userId)
        {
            return await _documentRepository.ListDocumentPermissionsWithUsers(userId);
        }

        public async Task<IEnumerable<UserAccessGroupsResponse>> GetUserAccessGroups(Guid? userId)
        {
            var queryData = await _userAccessGroupRepository.GetUserAccessGroups(userId);

            var results = queryData.GroupBy(u => u.UserId)
                 .Select(g => new UserAccessGroupsResponse
                 {
                     UserId = g.Key,
                     UserName = g.First().UserName,
                     AccessGroups = g.Select(p => p.AccessGroups.FirstOrDefault()).ToList()
                 })
                 .ToList();

            return results;
        }

        public async Task<bool> RemoveUserAccessFromDocument(RemoveUserAccessToDocumentRequest userDoc)
        {
            var command = new RemoveDocumentAccessUser(userDoc.UserId, userDoc.DocumentId);
            var result = await _docAccessUserRepository.RemoveDocPermissionForUserAsync(command);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserAccessGroupFromDocument(RemoveAccessGroupToDocumentRequest userGroup)
        {
            var command = new RemoveDocumentAccessGroup(userGroup.DocumentId, userGroup.AccessGroupId);
            var result = await _docAccessGroupRepository.RemoveDocPermissionFromGroupAsync(command);
            return result.Succeeded;
        }
    }
}
