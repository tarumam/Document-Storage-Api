using DocStorageApi.Data.Commands;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository.Interfaces
{
    public interface IDocumentRepository
    {
        Task<CommandResult<Guid?>> AddNewDocumentAsync(InsertDocumentCommand newDocument);
        Task<CommandResult<int>> DisableDocumentAsync(DisableDocumentCommand command);
        Task<IEnumerable<DocumentsResponse>> GetAllDocumentsAsync();
        Task<IEnumerable<DocumentPermissionsWithUserResponse>> ListDocumentPermissionsWithUsers(Guid? userId);
    }
}
