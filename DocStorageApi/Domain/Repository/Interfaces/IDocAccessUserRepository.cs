using DocStorageApi.Data.Commands;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository.Interfaces
{
    public interface IDocAccessUserRepository
    {
        Task<IEnumerable<DocumentsRelatedToUserResponse>> GetAllDocumentsAssociatedToUsersAsync(Guid userId);
        Task<CommandResult<int>> GrantDocPermissionForUserAsync(AssignUserToDocumentCommand command);
        Task<CommandResult<int>> RemoveDocPermissionForUserAsync(RemoveDocumentAccessUser command);
    }
}
