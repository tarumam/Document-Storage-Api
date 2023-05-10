using DocStorageApi.Data.Commands;
using DocStorageApi.DTO;
using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository.Interfaces
{
    public interface IDocAccessGroupRepository
    {
        Task<IEnumerable<DocumentAccessGroupsResponse>> GetAllGroupsAssociatedToDocumentAsync(Guid documentId);
        Task<IEnumerable<DocumentAccessGroupsResponse>> GetAllDocumentsAssociatedToGroupAsync(Guid groupId);
        Task<CommandResult<int>> GrantDocPermissionForGroupAsync(AssignGroupToDocumentCommand command);
        Task<CommandResult<int>> RemoveDocPermissionFromGroupAsync(RemoveDocumentAccessGroup command);

    }
}
