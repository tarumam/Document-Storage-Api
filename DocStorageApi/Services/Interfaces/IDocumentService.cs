using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<bool> AddNewDocumentAsync(AddNewDocumentRequest newDocument);

        Task<bool> DisableDocumentAsync(Guid documentId);

        Task<IEnumerable<DocumentsResponse>> GetAllDocumentsAsync();

    }
}
